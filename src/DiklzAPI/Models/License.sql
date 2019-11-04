SELECT	
		lic.DOC_ID				as Id,
		lic.LICCAT_ID,									-- Категория лицензии
		lic.LICSTAT_ID			as LicenseStatusId,		-- ИД статуса
		doc.REG_NUM				as RegNumber,			-- № ліцензії
		CONVERT(VARCHAR, doc.REG_DATE, 126)         
								as RegistrationDate,	-- Дата початку дії
		lic.IS_DISTRIB,									-- МПД в декількох регіонах														
		lic.ISSUE_ORDER_NUM,							-- № і дата наказу
		irs.ISSUE_REASON_NAME,							-- Підстава
		lic.ISSUE_ORDER_TEXT,							-- Текст наказу
		ISNULL(CONVERT(varchar, lic.END_DATE, 126), 'Безстроково')
								as END_DATE_STR,		-- Дата закінчення дії ліцензіі								
		CONVERT(varchar, lic.END_ORDER_DATE, 126)		as TerminateDate,	-- Дата анулювання ліцензії
		(substring(cast(
		(SELECT	', ' + clt.LICTYPE_NAME
			FROM	LIC_LICENSE_LICTYPE llt
			join	CDC_LICENSE_TYPE clt on clt.LICTYPE_ID = llt.LICTYPE_ID
		WHERE llt.DOC_ID	= lic.DOC_ID
		FOR XML PATH(''), type) as varchar(max)), 2, 8000)) 
								as LicenseTypesName, 
		org.EDRPOU				as EDRPOU,			    -- ЄДРПОУ/ід.код ліцензіата
		org.SIDE_NAME			as OrganizationName,	-- Назва ліцензіата
		org.ADDRESS				as OrganizationAddress,	-- Адреса ліцензіата
		org.ADDRESS_IDX			as POST_INDEX,			-- Поштовий індекс
		org.PHONE				as CONTACTS,		    -- Контактна інформація (тел., факс)
		org.PASSPORT			as SIDE_PASSPORT,		-- Паспортні дані (для фіз. особи)
		org.BANK_ACCOUNT		as SIDE_BANK_ACCOUNT,	-- Банківські реквізити 
		org.DIRECTOR_PIB		as SIDE_DIRECTOR_PIB,	-- ПІБ керівника
		reg.REGION_CODE			as SIDE_REGION_CODE	,	-- Код Регіону
		ISNULL(prg.REGION_NAME + ' ' + reg.REGION_NAME, reg.REGION_NAME)
								as SIDE_REGION_NAME,

		ofm.ORGFORM_NAME		as OrganizationFormName,-- Тип особи
		org.ORGFORM_ID			as SIDE_ORGFORM_ID,
		otp.OPFG_TYPE_ID,
		otp.OPFG_NAME,									-- Організаційно-правова форма
		owt.OWNERSHIP_TYPE_ID,          
		owt.OWNERSHIP_TYPE_NAME,				        -- Форма власності
		kvd.KVED_NAME,									-- Код виду економічної діяльності
		spd.SPODU_NAME,									-- Код відомчої підпорядкованості
  
------------------------------------------------------------------------------------------------------------------------
	(   SELECT			
				doc.DOC_ID				    AS 'Id' ,				-- Унікальний ідентифікатор МПД	
				lic.DOC_ID				    AS 'LicenseId',			-- Унікальний ідентифікатор ліцензії
				cls.LICSTAT_NAME            AS 'StatusName',		-- Статус дії МПД
				dsd.SIDE_NAME               AS 'Name',				-- Назва філії
				btp.[TYPE_NAME]             AS 'Type',				-- Тип філії
				(substring(cast(
				(SELECT ', ' + clt.LICTYPE_NAME
				FROM LIC_LICENSE_LICTYPE llt
				join CDC_LICENSE_TYPE clt on clt.LICTYPE_ID = llt.LICTYPE_ID
				WHERE llt.DOC_ID	= lic2.DOC_ID 
				FOR XML PATH(''), type)		as varchar(max)), 2, 8000)) 
											as 'LicTypes',			-- Перелік видів робіт
				regbra.REGION_CODE          as 'RegionCode',		-- Код території
				res.RESIDENCE_TYPE_NAME     as 'ResidenceTypeName',	-- Тип населеного пункту філії
				dsd.ADDRESS_IDX             as 'PostIndex',			-- Поштовий індекс
				dsd.[ADDRESS]               as 'Address',			-- Місце провадження діяльності
				CONVERT(VARCHAR, lic2.REG_IN_DATE, 126)		
											as 'RegistrationDate',	-- Дата включення в реєстр
				CONVERT(VARCHAR, lic2.END_ORDER_DATE, 126) 
											as 'TerminateDate'		-- Дата наказу про ліквідацію чи припинення діяльності

		FROM	LIMS_DOC doc
				JOIN LIC_DOC_LICENSE			lic2	on		lic2.DOC_ID				= doc.DOC_ID 
				JOIN LIMS_DOC_SIDE				dsd		on		dsd.DOC_ID				= doc.DOC_ID
					AND											dsd.DOCSIDE_TYPE_ID		= (SELECT _KEY FROM CONST_DST2_RECEIVER)
				LEFT JOIN CDC_LICENSE_STATUS	cls		on		lic2.LICSTAT_ID			= cls.LICSTAT_ID
				LEFT JOIN CDC_LIC_BRANCH_TYPE	btp		on		btp.[TYPE_ID]			= lic2.BRANCH_TYPE_ID
				LEFT JOIN CDC_REGION			regbra	on		regbra.REGION_ID		= dsd.REGION_ID
				LEFT JOIN CDC_RESIDENCE_TYPE	res		on		res.RESIDENCE_TYPE_ID	= lic2.RESIDENCE_TYPE_ID
		WHERE	(1=1)
			AND	doc.PARENT_ID		= lic.DOC_ID 
			AND	doc.DOC_TYPE_ID		= (SELECT _KEY FROM CONST_DT06_LICCOPY) 
			AND	lic2.LICSTAT_ID	IN ((SELECT _KEY FROM CONST_LICSTATE1_ACTIVE), (SELECT _KEY FROM CONST_LICSTATE4_TERMINATED))	
		FOR XML PATH('Branch'), TYPE)

		as ListOfBranchesString							-- Перелік МПД
------------------------------------------------------------------------------------------------------------------------


FROM	LIC_DOC_LICENSE					lic
		JOIN LIMS_DOC					doc on	doc.DOC_ID				= lic.DOC_ID
		JOIN LIMS_DOC_SIDE				org	on	org.DOC_ID				= doc.DOC_ID 
			AND						-- контрагент будет получателем лицензии
										DOCSIDE_TYPE_ID					= (SELECT _KEY FROM CONST_DST2_RECEIVER)
		LEFT JOIN CDC_OPFG_TYPE			otp on	otp.OPFG_TYPE_ID		= lic.OPFG_TYPE_ID
		LEFT JOIN CDC_REGION			reg on	reg.REGION_ID			= org.REGION_ID
		LEFT JOIN CDC_ORG_FORM			ofm on ofm.ORGFORM_ID			= org.ORGFORM_ID
		LEFT JOIN CDC_OWNERSHIP_TYPE	owt	on owt.OWNERSHIP_TYPE_ID	= lic.OWNERSHIP_TYPE_ID
		LEFT JOIN CDC_KVED				kvd on kvd.KVED_ID				= lic.KVED_ID
		LEFT JOIN CDC_SPODU				spd on spd.SPODU_ID				= org.SPODU_ID
		LEFT JOIN CDC_LIC_ISSUE_REASON	irs on irs.ISSUE_REASON_ID		= lic.ISSUE_REASON_ID
		LEFT JOIN CDC_REGION			prg on prg.REGION_ID			= reg.PARENT_ID
WHERE	(1=1)
AND		doc.DOC_TYPE_ID = (SELECT _KEY FROM CONST_DT05_LICENSE)
AND		(ISNULL('2,3,4','') = '' OR  
   -- лицензия выбирается если во множественном выборе выбран хотя бы один вид деятельности соответствующий лицензии
		EXISTS (SELECT 1
				FROM LIC_LICENSE_LICTYPE lct
				WHERE lct.DOC_ID = doc.DOC_ID
				AND CHARINDEX(','+CAST(lct.LICTYPE_ID as varchar)+',', ','+'2,3,4'+',') > 0
				)
		)