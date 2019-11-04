select 
   fr.id, 
   ri.caption,
   fr.right_id, 
   fr.role_id,   
   ri.is_active,
   ri.entity_access_level 
from sec_role_right as fr
join sec_right      as ri on ri.id=fr.right_id