SELECT
	feedbacks.ID,
    COALESCE ( feedbacks.caption, '') AS caption,
	feedbacks.app_id,
    feedbacks.app_sort,
    feedbacks.rating,
    feedbacks.is_rated,
    feedbacks.comment,
    feedbacks.org_id,
    feedbacks.org_employee_id
FROM
	feedbacks 
WHERE
	feedbacks.record_state <> 4