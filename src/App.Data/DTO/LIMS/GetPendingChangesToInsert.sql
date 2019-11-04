SELECT [Id]
      ,[EntityName]
      ,[EntityId]
      ,[Action]
      ,[Processed]
      ,[Created]
  FROM [dbo].[PendingChanges]
  WHERE [Processed] = 0  and  Action ='INSERT'