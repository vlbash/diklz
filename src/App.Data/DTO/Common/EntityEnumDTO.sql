select
    en_en_rec.id          as id,
    app.id                as application_id,
    br.id                 as branch_id,
    en_en_rec.entity_type as entity_type,
    en_rec.code           as enum_code,
    en_rec.name           as enum_name,
    en_rec.id             as enum_record_id,
    null                  as caption,
    en_rec.ex_param1 as   ex_param1,
    en_rec.ex_param2 as   ex_param2

from entity_enum_recordses as en_en_rec
left join trl_applications as app on app.id = en_en_rec.entity_id
left join org_branches as br on br.id = en_en_rec.entity_id
left join enum_record as en_rec on en_rec.code = en_en_rec.enum_record_code