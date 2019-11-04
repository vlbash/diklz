           --from [PRL_APP_NOTICE_DETAIL]
           --used to one-time import all the protocols

	SELECT  lic.DOC_ID as Id,
            lic.END_ORDER_NUM as EndOrderNumber,							-- № і дата наказу
			lic.END_ORDER_DATE as EndOrderDate,
			ers.END_REASON_NAME as EndReasonText,							-- Підстава
			lic.END_ORDER_TEXT as EndOrderText								-- Текст наказу
	  FROM	-- повідомлення
			LIC_DOC_LICENSE					lic
			LEFT JOIN CDC_LIC_END_REASON	ers on ers.END_REASON_ID		= lic.END_REASON_ID