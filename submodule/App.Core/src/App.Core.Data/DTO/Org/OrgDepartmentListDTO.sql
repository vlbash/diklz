select
 x.caption,
 x.id,
 x.name
from
 org_department as x
where ( x.record_state <> 4 )