select
 x.id,
 x.name as caption,
 x.category as category
from org_organization as x
   where x.record_state <> 4