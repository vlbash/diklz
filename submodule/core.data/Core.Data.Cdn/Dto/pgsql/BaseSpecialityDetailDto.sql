select
  x.id,
  x.caption,
  x.parent_id,
  x.description,
  coalesce( parent.caption, '') as parent_name
from cdn_speciality x
  left join cdn_speciality as parent on parent.id = x.parent_id and parent.record_state <> 4
where x.record_state <> 4