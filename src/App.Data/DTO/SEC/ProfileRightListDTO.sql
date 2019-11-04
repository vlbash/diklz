select 
    prr.id, 
    rig.caption, 
    prr.profile_id,
    prr.right_id,
    rig.is_active
from   sec_profile_right as prr
join   sec_right         as rig on rig.id = prr.right_id