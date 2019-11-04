           --from [PRL_APP_NOTICE_DETAIL]
           --used to one-time import all the protocols

	SELECT  apn.DOC_ID		    as NOTICEID	,
			apd.PARENT_ID		as APPID		,
			apd.OWNER_ORG_ID	as OWNERID		,
			apd.REG_NUM			as REGNUM,
			apd.REG_DATE		as REGDATE,
			apt.APP_TYPE_NAME	as APPTYPENAME,
			lad.REG_NUM			as APPREGNUM	,
			lad.REG_DATE		as APPREGDATE	,
			lds.SIDE_NAME		as SIDENAME,
			apn.STATUS_ID		as STATUSID,
			ans.STATUS_NAME		as STATUSNAME,
			apd.SIGNER_PIB		as SIGNERPIB,
			apd.SIGNER_POS		as SIGNERPOS,
			-- аудит
			apd.CREATE_DATE		as CREATEDATE,
			apd.CREATOR_NAME	as CREATORNAME,
			-- Відповідальний виконавець
			ldp.PERSON_ID       as PERSONID,
			dpp.LAST_NAME_N	
				+ ISNULL(' ' + SUBSTRING(dpp.FIRST_NAME_N,1,1)+'.', '') 
				+ ISNULL(SUBSTRING(dpp.MIDDLE_NAME_N,1,1)+'.', '')
								as PERSONNAME
	  FROM	-- повідомлення
			LIC_APP_NOTICE					apn
			JOIN LIMS_DOC					apd	on	apd.DOC_ID		= apn.DOC_ID
												and apd.DOC_TYPE_ID	= (SELECT _KEY FROM CONST_DT87_PRL_APP_NOTICE)
			JOIN CDC_LIC_APP_NOTICE_STATUS	ans	on	ans.STATUS_ID	= apn.STATUS_ID
			-- заява
			JOIN LIMS_DOC					lad	on	lad.DOC_ID		= apd.PARENT_ID
			JOIN LIMS_DOC_SIDE				lds on	lds.DOC_ID		= lad.DOC_ID						
												and lds.DOCSIDE_TYPE_ID	 = (SELECT _KEY FROM CONST_DST1_SENDER)
			JOIN LIC_APP					lap	on	lap.DOC_ID		= lad.DOC_ID
			JOIN CDC_LIC_APP_TYPE			apt	on	apt.APP_TYPE_ID	= lap.APP_TYPE_ID

	   LEFT JOIN LIMS_DOC_PERSON			ldp on	ldp.DOC_ID		= apd.DOC_ID
	   LEFT JOIN DICT_PERSON				dpp on	dpp.PERSON_ID	= ldp.PERSON_ID