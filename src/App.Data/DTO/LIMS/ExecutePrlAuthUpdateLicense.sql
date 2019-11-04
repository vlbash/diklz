begin
declare @temp int = (select BRANCH_LIC_ID from LIC_APP_BRANCH where BRANCH_APP_ID = @p_LicenseId)

EXECUTE [dbo].[CRV_LICENSE_AUTH_PERSON_INSERT] --[dbo].[PRL_APP_AUTH_PERSON_UPDATE] для получения ид из старого лимса
   @p_PersonId OUTPUT
  --,@p_LicenseId
  ,@temp
  ,@p_PersonName
  ,@p_PersonPos
  ,@p_Education
  ,@p_ContactInfo
  ,@p_Email
  ,@p_Notes
  ,@p_Used
  end