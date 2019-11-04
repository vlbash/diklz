select 
    prr.id, 
    rol.caption, 
    prr.profile_id,
    prr.role_id,
    rol.is_active
from   sec_profile_role  as prr
join   sec_role          as rol on rol.id = prr.role_id