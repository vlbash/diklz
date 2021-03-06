SELECT
	brn.ID,
	COALESCE ( brn.NAME, '' ) AS NAME,
    COALESCE ( brn.phone_number, '' ) AS phone_number,
	COALESCE ( brn.caption, '' ) AS caption,
	COALESCE (( SELECT msg.org_unit_id FROM messages AS msg WHERE msg.record_state <> 4 AND msg.ID = apb.lims_document_id ), '00000000-0000-0000-0000-000000000000' ) AS org_unit_id,
	COALESCE ( apb.lims_document_id, '00000000-0000-0000-0000-000000000000' ) AS message_id,
	brn.branch_activity 
FROM
	org_branches AS brn
	JOIN application_branches AS apb ON apb.branch_id = brn.ID 
WHERE
	brn.record_state <> 4