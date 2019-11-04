begin
declare @temp int = (select BRANCH_LIC_ID from LIC_APP_BRANCH where BRANCH_APP_ID = @p_BranchAppId)

EXECUTE [dbo].[IML_APP_BRANCH_PERSON_UPDATE] 
	@p_PersonId		,
	@p_BranchAppId	,
    --@temp           ,
	@p_PersonName	,
	@p_PersonPos	,
	@p_Education	,
	@p_Phone		,
	@p_Fax			,
	@p_Email		,
	@p_Experience	, 
	@p_ContractInfo	,
	@p_Notes		
end