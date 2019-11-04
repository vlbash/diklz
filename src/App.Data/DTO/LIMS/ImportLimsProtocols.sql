           --from [LOAD_LIC_PROTOCOL]
           --used to one-time import all the protocols


SELECT	    prt.DOC_ID		as OLDLIMSID, 
		    doc.REG_NUM		as PROTOCOLNUMBER,	--	№ протоколу
		    doc.REG_DATE	as PROTOCOLDATE,	--  Дата протоколу
		    doc.DOC_NUM		as ORDERNUMBER,	    --	№ наказу
		    doc.DOC_DATE	as ORDERDATE,		--  Дата наказу
			prt.STATUS_ID   as STATUSID,
			pst.STATUS_NAME as STATUSNAME,
			case when doc.DOC_TYPE_ID = 85 then 'PRL'
			     when doc.DOC_TYPE_ID = 99 then 'IML' 
				 when doc.DOC_TYPE_ID = 70 then 'TRL' end as TYPE

FROM	    LIC_PROTOCOL            prt
    JOIN    LIMS_DOC                doc on doc.DOC_ID = prt.DOC_ID			
    JOIN    CDC_LIC_PROTOCOL_STATUS pst on pst.STATUS_ID = prt.STATUS_ID
WHERE  doc.DOC_TYPE_ID	in  (85, 99,70) 
	 ORDER	BY 1 DESC