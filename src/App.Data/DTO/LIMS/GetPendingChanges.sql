select 
[Id]
      ,[EntityName]
      ,[EntityId]
      ,[Action]
      ,[Processed]
      ,[Created]
	  from(
SELECT ROW_NUMBER() over(partition by EntityId order by id desc) as rownum,
       [Id]
      ,[EntityName]
      ,[EntityId]
      ,[Action]
      ,[Processed]
      ,[Created]
  FROM [dbo].[PendingChanges]
  WHERE [Processed] = 0
  and EntityName =@__entityName_1 
  )A
  where rownum = 1 