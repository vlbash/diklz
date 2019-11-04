	---PRL_CHECK_DETAIL
    
    SELECT	chk.CHECK_ID			as CheckId		,
			-- Основні дані про СГД
			--dsd.LIC_DOC_ID			as LIC_ID		,
			--dsd.SIDE_NAME			as SGD_NAME		,
			--dsd.EDRPOU				as SGD_EDRPOU	,
			--dsd.[ADDRESS]			as SGD_ADDRESS	,
			--ldc.REG_DATE			as LIC_DATE		,
			--ldc.REG_NUM				as LIC_NUM		,
			--lic.ISSUE_ORDER_DATE					,
			---- Основні дані про перевірку
			--chk.BEGIN_DATE							,	-- Планова дата початку 
			--chk.END_DATE							,	-- Планова дата кінця перевірки 
			--chk.CREATOR_NAME						,	-- Перевірку створив
			--chk.CREATE_DATE							,	-- Дата створення перевірки
			--chk.NOTES								,
			---- Заява щодо ліцензування
			--chk.APP_ID								,	
			--adc.REG_NUM				as APP_NUM		,
			--adc.REG_DATE			as APP_DATE		,				
			--atp.APP_TYPE_ID							,
			--atp.APP_TYPE_NAME						,
			---- Секція «Виконання перевірки	
			--chk.ORDER_NUM							,	-- № наказу на перевірку
			--chk.ORDER_DATE							,	-- Дата наказу на перевірку
			--chk.ORDER_PERFORMER						,	-- Виконавець в документах
			--chk.IDENTITY_NUM						,	-- № посвідчення
			--chk.IDENTITY_DATE						,	-- Дата посвідчення
			-- Секція «Результат перевірки»
			chk.FACT_DATE							as FactDate,	-- Дата закінчення перевірки
			chk.DEFECT_COUNT						as DefectCount	-- Кількість порушень 
			--chk.ACT_TYPE_ID							,	-- Тип акту (Id)
			--ptp.ACT_TYPE_NAME						,	-- Тип акту	
			--chk.ACT_NUM									-- № акту перевірки
	 FROM	PRL_CHECK	chk
			LEFT JOIN CDC_PRL_ACT_TYPE  ptp on ptp.ACT_TYPE_ID			= chk.ACT_TYPE_ID
				 JOIN LIC_APP			app on app.DOC_ID				= chk.APP_ID
				 JOIN CDC_LIC_APP_TYPE  atp on atp.APP_TYPE_ID			= app.APP_TYPE_ID
				 JOIN LIMS_DOC			adc on adc.DOC_ID				= app.DOC_ID						
				   							and adc.DOC_TYPE_ID			= (SELECT _KEY FROM CONST_DT86_PRL_APP)
				-- Заявник
				 JOIN LIMS_DOC_SIDE		dsd on dsd.DOC_ID				= app.DOC_ID  
											and dsd.DOCSIDE_TYPE_ID		= (SELECT _KEY FROM CONST_DST1_SENDER)
			LEFT JOIN LIC_DOC_LICENSE	lic on lic.DOC_ID				= dsd.LIC_DOC_ID
			LEFT JOIN LIMS_DOC			ldc on ldc.DOC_ID				= lic.DOC_ID