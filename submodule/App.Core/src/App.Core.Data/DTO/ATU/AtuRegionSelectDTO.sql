select
	a.id,
	a.name as caption,
	a.parent_id
from atu_region as a
where a.record_state <> 4