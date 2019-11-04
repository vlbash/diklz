EXECUTE [dbo].[TRL_LICENSE_PROCESS_RUN] 
	@p_LicDocId			 output,		-- Ид созданной или обновленной лицензии
	@p_AppId			, 			-- Ід заявки
	@p_LicRegNum		,	-- Номер нової ліцензії (№ ліцензії, що визнається недійсною)
	@p_NewLicRegNum		,	-- Номер нової ліцензії
	@p_NewLicStartDate	,		-- Дата початку дії
	@p_NewLicEndDate	,		-- Дата закінчення дії
	@p_DecisionText		,	-- Текст рішення для ліцензії
	@p_NewDecisionText	,	-- Текст рішення для нової ліцензії 
	@p_SignerPos		,	-- Підписав - посада
	@p_SignerPib		,	-- Підписав - ПІБ
	@p_ModifyDate		,	-- Дата внесення в реєстр
	@p_SessionId		,			-- сессия (для записи в таблицу аудита)
	@p_OrgId			,			-- ИД организации пользователя (если есть)
	@p_UserId			,			-- ИД пользователя (если есть)
	@p_Domain			,	-- прикладной домен (если есть)
	@p_UserName			,	-- имя пользователя
	@p_UserFullName			-- ФИО пользователя