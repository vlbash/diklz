SELECT 
	org_employee.id,
    org_employee.organization_id                                        AS org_unit_id,
	COALESCE(org_employee.caption, '') 									AS caption,
	COALESCE ((
		SELECT
			concat ( person.last_name, ' ', person.NAME, ' ', person.middle_name ) 
		FROM
			person 
		WHERE
			person.ID = org_employee.person_id 
			),
		    '')                                                         AS recipient_name, 
    org_employee.receive_on_change_all_application,
    org_employee.receive_on_change_all_message,
    org_employee.receive_on_change_org_info,
    org_employee.receive_on_overdue_payment,
	org_employee.user_email	                                            AS recipient_email
FROM 
	org_employee
WHERE 
    (org_employee.record_state <> 4) 