select 

    ID,
    person_id                              as person_id,
    coalesce(caption, '')                  as caption,
    organization_id                        as org_id,
    user_email                             as user_email,
    position                               as position,
    receive_on_change_all_application,
    receive_on_change_all_message,
    receive_on_change_own_application,
    receive_on_change_own_message,
    personal_cabinet_status,
    receive_on_change_org_info,
    receive_on_overdue_payment

from org_employee

where record_state <> 4