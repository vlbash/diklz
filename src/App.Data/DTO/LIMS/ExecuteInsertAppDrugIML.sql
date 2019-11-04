EXECUTE [dbo].[IML_APP_DRUG_INSERT]
	@p_DrugId				OUTPUT,			-- Ідентифікатор ЛЗ
	@p_AppId				,				-- Ідентифікатор заяви
	@p_PosNumber			,				-- № з.п.		
	@p_RpNumber				,		-- № РП ЛЗ
	@p_DrugName				,		-- Торгов. назва
	@p_FormtypeDesc			,		-- Лікарська форма
	@p_ActiveDose			,		-- Доза діючої речовини в кожній одиниці
	@p_CountInPack			,		-- Кількість одиниць в упаковці
	@p_DrugMnn				,		-- Міжнародна непатентована назва (МНН)
	@p_AtcCode				,		-- Код ATC
	@p_ProducerName			,		-- Назва виробника
	@p_ProducerCountry		,		-- Країна виробника
	@p_SupplierName			,		-- Назва постачальника
	@p_SupplierCountry		,		-- Країна постачальника
	@p_SupplierAddress		,		-- Адреса постачальника
	@p_Notes				,		-- Примітки
	@p_IsLicense			,				-- Включити в ліцензію
	@p_IsProblem			,				-- Є проблеми
	@p_ProblemInfo					-- Проблеми