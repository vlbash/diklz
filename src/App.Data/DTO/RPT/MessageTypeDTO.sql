SELECT                      
	mes.id                              as Id,
    mes.org_unit_id                     as org_unit_id,
    mes.message_type                    as mess_type,
    coalesce(mes.caption, '')           as caption

from
	messages as mes