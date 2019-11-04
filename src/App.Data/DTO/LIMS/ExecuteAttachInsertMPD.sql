begin
declare @temp int = (select BRANCH_LIC_ID from LIC_APP_BRANCH where APP_ID = @p_DocId)

EXECUTE [dbo].[COMMON_ATTACH_INSERT]
        @p_FileId OUTPUT
        ,@p_FileTypeId
        ,@p_EntityId
        ,@p_EntityTypeId
        ,@temp
        ,@p_FileName
        ,@p_FileSize
        ,@p_FileDate
        ,@p_Description
        ,@p_CreatorName
        ,@p_FileDocNum
        ,@p_FileDocDate
        end