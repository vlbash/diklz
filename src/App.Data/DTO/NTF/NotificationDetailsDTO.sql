SELECT
	notifications.ID,
    COALESCE ( notifications.caption, '') AS caption,
	notifications.parent_id,
	COALESCE ( notifications.date_of_create, notifications.created_on ) AS date_of_create,
	COALESCE ((
		SELECT
			concat ( person.last_name, ' ', person.NAME, ' ', person.middle_name ) 
		FROM
			person 
		WHERE
			person.ID = notifications.created_by 
			),
		'' 
	) AS user_create,
	COALESCE ((
		SELECT
			concat (
				person.last_name,
				' ',
				SUBSTRING ( person.NAME, 1, 1 ),
				'. ',
				SUBSTRING ( person.middle_name, 1, 1 ),
				'.' 
			) 
		FROM
			person 
		WHERE
			person.ID = notifications.created_by 
			),
		'' 
	) AS user_create_pib,
	notifications.notification_subject,
	notifications.notification_text,
    notifications.notification_type,
	notifications.recipient_json_list,
	notifications.is_send 
FROM
	notifications 
WHERE
	notifications.record_state <> 4