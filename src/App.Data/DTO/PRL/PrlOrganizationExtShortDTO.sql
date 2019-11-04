select  
    org.id,
    coalesce(org.edrpou,'') as edrpou,
    coalesce(org.inn,'') as inn,
    coalesce(org.caption,'') as caption,
    coalesce(ooi."name", '') as "name"
    
from org_organization as org
join org_organization_info ooi
    on ooi.organization_id = org.id
    and ooi.is_actual_info = true
    and ooi.type = 'PRL'

where org.record_state <> 4