﻿begin
declare @temp int = (select BRANCH_LIC_ID from LIC_APP_BRANCH where BRANCH_APP_ID = @p_LicDocId)

EXECUTE [dbo].[IML_APP_BRANCH_ADD] 
   @p_AppId
  ,@temp
  ,@p_BranchAppId
end