EXECUTE [dbo].[TRL_LIC_APP_BRANCH_INSERT]
	@p_BranchAppId			 output,			-- Ідентифікатор філії у  заяві
	@p_BranchLicId			,				-- Ідентифікатор копії ліцензії
	@p_AppId				,				-- Ідентифікатор заяви
	@p_BranchNumber			,				-- № філії з/п	
	@p_BranchTypeId			,				-- Тип філії	
	@p_BranchName			,		-- Назва філії
	@p_RegionId				,				-- Код території	
	@p_ResidenceTypeId		,				-- Тип населеного пункту	
	@p_BranchAddress		,		-- Місце провадження діяльності
	@p_BranchTypeIds		,		-- Перелік видів робіт
	@p_AseptId				,				-- Асептичні умови	
	@p_BranchAddressIdx		,		-- Поштовий індекс
	@p_Phone				,		-- Контактна інформація (тел., факс)
	@p_SpecialConditions	,		-- Особливі умови
	@p_Remarks				,		-- Зауваження
	@p_ToCheck						
