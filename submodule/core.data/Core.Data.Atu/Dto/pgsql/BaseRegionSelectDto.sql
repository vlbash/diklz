select
	a.id,
	a.caption,
	a.parent_id
from atu_region as a
where a.record_state <> 4