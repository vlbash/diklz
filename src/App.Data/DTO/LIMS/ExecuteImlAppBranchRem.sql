begin
declare @temp int = (select BRANCH_LIC_ID from LIC_APP_BRANCH where BRANCH_APP_ID = @p_ID)

EXECUTE [dbo].[CRV_LICENSE_COPY_DELETE] 
   @temp
  ,@err_code
  ,@err_msg
end

