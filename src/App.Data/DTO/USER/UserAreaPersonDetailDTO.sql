select 

    ID,
    coalesce(caption, '')           as caption,
    name,
    middle_name,
    last_name,
    phone,
    email,
    ipn

from person

where record_state <> 4