begin
declare @temp int = (select BRANCH_LIC_ID from LIC_APP_BRANCH where BRANCH_APP_ID = @p_LicenseId)

EXECUTE [dbo].[CRV_LICENSE_AUTH_PERSON_UPDATE2]
   @p_PersonIds
  ,@temp 
  end
