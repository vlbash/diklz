 SELECT     --top 10
            rp.DOC_ID               as DocId,
			rp.DRUG_NAME_UKR		as DrugNameUkr,			-- Назва ЛЗ*
			rp.FORMTYPE_ID          as FormtypeId,
			frm.FORM_NAME			as FormName,			-- Тип форми випуску
			rp.FORMTYPE_DESC		as FormtypeDesc,		-- Форма випуску
			org.SIDE_NAME			as SideName,			-- Заявник
			cnt.COUNTRY_ID			as CountryId,
			cnt.NAME_SHORT			as CountryName, 		-- Країна заявника
			rp.IS_RESIDENT			as IsResident,			-- Виробник- резидент
			rp.PRODUCER_NAME		as ProducerName,		-- Виробник
			rp.COUNTRY_NAME			as ProdCountryName, 	-- Країна виробникa				изменено 10.06.2011 by SA: Req. № 464
			rp.REG_PROCEDURE 		as RegProcedure,		-- Реєстраційна процедура
			prc.REGPROC_ID          as RegprocId,
			prc.REGPROC_NAME		as RegprocName,			-- Тип реєстраційної процедури
            prc.REGPROC_CODE        as RegprocCode,
			typ.DRUGTYPE_ID         as DrugtypeId,
			typ.DRUGTYPE_NAME		as DrugtypeName,		-- Тип ЛЗ
			cls.DRUG_CLASS_ID       as DrugClassId,
			cls.DRUG_CLASS_NAME		as DrugClassName,		-- Тип препарату
			rp.RP_ORDER_ID          as RpOrderId,
			ord.REG_NUM				as OrdRegNum,   		-- № наказу Фармцентру*
			ord.REG_DATE			as OrdRegDate,  		-- Дата наказу Фармцентру*
			doc.REG_NUM				as RegNum,				-- № реєстраційного посвідчення*
			doc.REG_DATE			as RegDate,				-- Дата початку*
			rp.END_DATE				as EndDate,				-- Дата закінчення*
			rp.OFF_ORDER_NUM		as OffOrderNum,			-- № наказу про припинення дії РП
			rp.OFF_ORDER_DATE		as OffOrderDate,		-- Дата наказу про припинення дії РП
			rp.OFF_REASON			as OffReason,			-- Причина припинення дії РП
			rp.DRUG_NAME_ENG		as DrugNameEng,			-- МНН
			rp.ACTIVE_SUBSTANCES	as ActiveSubstances,	-- Склад діючих речовин
			rp.FARM_GROUP			as FarmGroup,			-- Клініко-фармакологічна група
			rp.SALE_TERMS			as SaleTerms,			-- Умови відпуску 
			rp.PUBLICITY_INFO		as PublicityInfo,		-- Рекламування
			doc.NOTES				as Notes,				-- Примітки	
			(	CASE
					-- термін дії РП ЛЗ більше поточної дати або дорівнює та дата наказу про припинення дії РП відсутня
					WHEN rp.END_DATE >= CAST(GETDATE() as date) AND rp.OFF_ORDER_DATE IS NULL	THEN (SELECT _KEY FROM CONST_CRV_RP_STATE01_ACTIVE)	-- Діє
					-- термін дії РП ЛЗ  менше поточної дати та дата наказу про припинення дії РП відсутня 
					WHEN rp.END_DATE <  CAST(GETDATE() as date) AND rp.OFF_ORDER_DATE IS NULL	THEN (SELECT _KEY FROM CONST_CRV_RP_STATE02_EXPIRE)	-- Термін закінчився
					-- дата наказу про припинення дії РП присутня 
					WHEN rp.OFF_ORDER_DATE IS NOT NULL											THEN (SELECT _KEY FROM CONST_CRV_RP_STATE03_CANCEL)	-- Припинено дію
				END
			)							as StateId, 		-- Стан дії РП
			rp.ATC_CODE					as AtcCode			-- код АТС
	  FROM	LIMS_RP rp
	        JOIN LIMS_DOC               doc on doc.DOC_ID			= rp.DOC_ID
										and doc.DOC_TYPE_ID		= (SELECT _KEY FROM CONST_DT02_CRVRP)
			LEFT JOIN CDC_DRUG_FORM     frm on frm.FORM_ID			= rp.FORMTYPE_ID
			LEFT JOIN LIMS_DOC_SIDE     org on org.DOC_ID			= rp.DOC_ID
			-- Країна заявника
			LEFT JOIN CDC_COUNTRY       cnt on cnt.COUNTRY_ID		= org.COUNTRY_ID
			LEFT JOIN CDC_DRUG_REGPROC  prc on prc.REGPROC_ID		= rp.REGPROC_ID
			LEFT JOIN CDC_DRUG_TYPE     typ on typ.DRUGTYPE_ID		= rp.DRUGTYPE_ID
			LEFT JOIN CDC_DRUG_CLASS    cls on cls.DRUG_CLASS_ID	= rp.DRUG_CLASS_ID
			LEFT JOIN LIMS_DOC          ord on ord.DOC_ID			= rp.RP_ORDER_ID