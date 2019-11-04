-- from [PRL_APP_SET_STATUS]
-- i'm not executing original procedure because
-- i'm not using bussiness logic checks from the original procedure


BEGIN    
    BEGIN TRY
		BEGIN TRANSACTION

		UPDATE [LIC_APP] SET
			STATUS_ID	= @p_StatusId
		WHERE
			DOC_ID		= @p_AppId

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

		DECLARE @ErrorMessage varchar(4000);
		SELECT @ErrorMessage = ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, 11, 1);
	END CATCH
END