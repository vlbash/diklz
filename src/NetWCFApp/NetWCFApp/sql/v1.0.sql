/****** Object:  Table [dbo].[PORTAL_LIC_APP_BRANCH]    Script Date: 1/31/2019 2:27:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PORTAL_LIC_APP_BRANCH](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LIC_ID] [int] NOT NULL,
	[LIC_BRANCH_ID] [int] NOT NULL,
	[APP_BRANCH_ID] [int] NOT NULL
	
 CONSTRAINT [PK_PORTAL_LIC_APP_BRANCH] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [Lims]
GO
/****** Object:  StoredProcedure [dbo].[TRL_LIC_APP_BRANCH_ADD]    Script Date: 1/31/2019 2:18:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- Author:		SV
-- Create date: 18.12.2011
-- Description:	Детальная форма заяви щодо ліцензування. Додавання підрозділу з переліку підрозділів
-- ============================================================================
CREATE OR ALTER PROCEDURE [dbo].[TRL_LIC_APP_BRANCH_ADD]	
	@p_LicAppId			int,
	@p_LicDocId			int
AS

BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRY
		BEGIN TRANSACTION		
						

		DECLARE 
			@v_BranchNumber int
			-- Номер філії при додаванні автоматично встановлюється як максиамальний
			-- серед усіх філій у заяві + 1
			SELECT @v_BranchNumber = ISNULL(MAX(BRANCH_NUMBER), 0) + 1
			  FROM LIC_APP_BRANCH
			 WHERE APP_ID = @p_LicAppId
		
		DECLARE
			@p_BranchAppId int

		-- 1. добавление основных данных из копии лицензии
		INSERT INTO LIC_APP_BRANCH  (
				APP_ID				,
				BRANCH_LIC_ID		,
				BRANCH_NUMBER		,
				BRANCH_NAME			,
				REGION_ID			,
				BRANCH_ADDRESS		,
				BRANCH_ADDRESS_IDX	,
				RESIDENCE_TYPE_ID	,
				ASEPT_ID,
				[TYPE_ID],
				PHONE,
				SPECIAL_CONDITIONS
			) 
		
		SELECT	@p_LicAppId				, 
				lic.DOC_ID				,
				@v_BranchNumber			,
				org.SIDE_NAME			,
				reg.REGION_ID			,
				org.ADDRESS				,
				org.ADDRESS_IDX			,
				ISNULL(lic.RESIDENCE_TYPE_ID,	(SELECT _KEY FROM CONST_RESTP3_UNKNOWN)),
				ISNULL(lic.ASEPT_ID,			(SELECT _KEY FROM CONST_TRL_ASC01_NOTDEFINE)),
				ISNULL(lic.BRANCH_TYPE_ID,		(SELECT _KEY FROM CONST_LIC_BRTP01_NOTDEFINE)),
				org.PHONE,
				lic.SPECIAL_CONDITIONS
		  FROM	LIC_DOC_LICENSE			lic
					 JOIN LIMS_DOC		dlc on dlc.DOC_ID			= lic.DOC_ID 
										   and dlc.DOC_TYPE_ID		in (SELECT _KEY FROM CONST_DT06_LICCOPY)
				LEFT JOIN LIMS_DOC_SIDE	org	on org.DOC_ID			= dlc.DOC_ID					 
										   and org.DOCSIDE_TYPE_ID	=  (SELECT _KEY FROM CONST_DST2_RECEIVER)
				LEFT JOIN CDC_REGION	reg	on	reg.REGION_ID		= org.REGION_ID
		 WHERE	lic.DOC_ID = @p_LicDocId
		
		SET @p_BranchAppId	= @@IDENTITY

		-- 2. добавление "Видів робіт" из копии лицензии
		INSERT INTO LIC_APP_BRANCH_LICTYPE (LICTYPE_ID, BRANCH_ID) 
			SELECT LICTYPE_ID, @p_BranchAppId
			  FROM LIC_LICENSE_LICTYPE
			 WHERE DOC_ID = @p_LicDocId

		-- 3. Перерахунок кількості філій в заяві
		UPDATE LIC_APP SET
		  BRANCH_COUNT = (SELECT COUNT(*) FROM LIC_APP_BRANCH WHERE APP_ID = @p_LicAppId)
		WHERE DOC_ID = @p_LicAppId

		-- 20.09.2012 add by Bashtovy (DIKLZ-4575)  
		DECLARE @v_AppTypeId int -- Тип заяви

		SELECT	@v_AppTypeId = app.APP_TYPE_ID
		FROM	LIC_APP app
				JOIN LIMS_DOC doc on doc.DOC_ID = app.DOC_ID
		WHERE	app.DOC_ID = @p_LicAppId

		IF (@v_AppTypeId = (SELECT _KEY FROM CONST_LIC_APT1_NEWLIC) OR @v_AppTypeId = (SELECT _KEY FROM CONST_LIC_APT2_COPYLIC))
		BEGIN
			UPDATE LIC_APP_BRANCH
				SET TO_CHECK = (SELECT _KEY FROM CONST_BOOL_TRUE) 
			WHERE APP_ID = @p_LicAppId
		END

		-- 4. 31.01.2019 mahurov
        --додавання запису у розв'язку для історії онов МПД
		INSERT INTO [PORTAL_LIC_APP_BRANCH](
				[LIC_ID],
				[LIC_BRANCH_ID],
				[APP_BRANCH_ID])
		SELECT	PARENT_ID,
				@p_LicDocId,
				@p_BranchAppId
		FROM	LIMS_DOC
		WHERE	DOC_ID = @p_LicDocId

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION

		DECLARE @ErrorMessage varchar(4000);
		SELECT @ErrorMessage = 'Помилка при спробі створення підрозділу в заяві щодо ліцензування ! ' + ERROR_MESSAGE();
		RAISERROR(@ErrorMessage, 11, 1);
	END CATCH
END