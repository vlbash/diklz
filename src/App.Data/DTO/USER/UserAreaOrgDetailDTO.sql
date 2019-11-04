select 

    ID,
    coalesce(caption, '')           as caption,
    name,
    edrpou,
    inn,
    email

from org_organization

where record_state <> 4