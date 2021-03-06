SELECT
	mes.ID,
	COALESCE ( mes.reg_date, '1900-01-01' ) AS reg_date,
	COALESCE ( mes.reg_number, '' ) AS reg_number,
	COALESCE ( mes.message_type, '' ) AS message_type,
	COALESCE ( mes.performer_id, '00000000-0000-0000-0000-000000000000' ) AS performer_id,
    mes.is_created_on_portal											as is_created_on_portal,
    mes.is_iml_license                                                  as is_iml_license,
    mes.is_prl_license                                                  as is_prl_license,
    mes.is_trl_license                                                  as is_trl_license,
	COALESCE ( mes.caption, '' ) AS caption 
FROM
	messages AS mes 
WHERE
	mes.record_state <> 4