EXECUTE [dbo].[TRL_LIC_APP_INSERT]
	-- Дані про заяву
	@p_LicAppId			 output,		-- Ідентифікатор заяви    
	@p_AppStatusId		,			-- Ідентифікатор статусу заяви
	@p_AppTypeId		,			-- Тип заяви
	@p_AppReasonIds  	,	-- Підстава(множинний вибір)
	@p_AppReasonId  	,			-- Підстава
	@p_SgdAppNum		,	-- № заяви СГД
	@p_SgdAppDate		,			-- Дата заяви СГД
	@p_RegAppNum		,	-- Реєстр. № заяви
	@p_RegAppDate		,			-- Реєстр. дата заяви
	@p_IsFree			,			-- Безкоштовно
	@p_Performer		,	-- Відповідальний виконавець
	@p_Notes			,	-- Примітки
	@p_BranchCount		,			-- Кількість діючих філій в ліцензії
	@p_IsActsReceived	,			-- Оригінали актів перевірки отримано
	-- Дані про ліцензію
	@p_LicDocId			,			-- Id ліцензії !!! (якщо не Null створюємо сторону заяви з типом CONST_DST2_RECEIVER, інакше - CONST_DST1_SENDER)
	@p_LictypeIds		,	-- Види діяльності(множинний вибір)
	@p_SideEdrpou		,	-- код ЕДРПОУ заявителя
	@p_OrgformId		,			-- ID типа особи
	@p_SideName			,	-- Назва юр. особи (ПІБ фіз. особи)
	@p_SideAddress		,	-- Адреса заявника(Адреса місця провадження діяльності)
	@p_SideIndex		,	-- Поштовий індекс
	@p_SideDirectorPib	,	-- ПІБ керівника
	@p_SideContacts		,	-- Контактна інформація (тел., факс)
	@p_BankAccount		,	-- Банківські реквізити (атрибут ліцензії !!!)
	@p_Passport			,	-- Паспортні дані
	-- Секція «Коди» 
	@p_RegionId			,			-- Код території (new)
	@p_OpfgTypeId		,			-- Організаційно-правова форма
	@p_OwnershipTypeId	,			-- Форма власності
	@p_KvedId			,			-- Код виду економічної діяльності
	@p_SpoduId			,			-- Код виду економічної діяльності
	-- Аудит
	@p_CreatorName		,
	@p_CreateDate			

    