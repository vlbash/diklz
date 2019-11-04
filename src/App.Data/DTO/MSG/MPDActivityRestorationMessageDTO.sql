select 
                                                                      --MESSAGE BASE DETAIL DTO START
    mes.id,
    mes.message_date                                                    as message_date,
    coalesce(mes.org_unit_id, '00000000-0000-0000-0000-000000000000')  as org_unit_id,
    mes.message_number                                                 as message_number,
    mes.message_type                                                   as message_type,
    mes.message_text                                                   as message_text,
    mes.is_iml_license                                                 as is_iml_license,
    mes.is_prl_license                                                 as is_prl_license,
    mes.is_trl_license                                                 as is_trl_license,
    mes.is_created_on_portal											as is_created_on_portal,
    coalesce(mes.caption, '')                                          as caption,
                                                                      --MESSAGE BASE DETAIL DTO END
    coalesce(mes.restoration_date,'1900-01-01')                         as restoration_date,
	mes.mpd_selected_id											       as mpd_selected_id,
    coalesce(mes.restoration_reason,'')                                 as restoration_reason
from messages as mes
where mes.record_state <> 4