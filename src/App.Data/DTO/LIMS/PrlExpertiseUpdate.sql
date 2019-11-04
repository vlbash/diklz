--part of PLR_APP_UPDATE

UPDATE LIC_APP
			SET EXP_RESULT_ID	 =  @p_ExpResultId	,
				EXP_DATE		 =  @p_ExpDate		,
				EXP_PERFORMER_ID =  @p_ExpPerformerId
		WHERE DOC_ID  = @p_AppId