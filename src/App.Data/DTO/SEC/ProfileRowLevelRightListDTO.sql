select 
    rlr.id, 
    rlr.caption, 
    rlr.profile_id,
    rls.entity_id,
    rlr.entity_name,
    rlr.access_type,
    rls.id as secur_id
from sec_row_level_right            as rlr
join sec_row_level_securtity_object as rls on rlr.id = rls.row_level_right_id