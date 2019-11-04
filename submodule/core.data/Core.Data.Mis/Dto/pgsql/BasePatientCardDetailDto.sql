select
x.caption,
x.enabled,
x.description,
x.reg_date,
x.reg_number,
x.id,
x.person_id,
person.last_name || ' ' || person.name|| ' ' ||   person.middle_name  as person_full_name,
person.last_name as person_last_name,
person.name as person_name,
person.middle_name as person_middle_name,
coalesce(person.ipn, '') as person_ipn,
person.phone as person_phone,
person.birthday as person_birthday,
person.email as person_email,
person.gender_enum as person_gender_enum,
(select name from enum_record  where  enum_type = 'Gender'  and code = person.gender_enum) as person_gender
from mis_patient_card as x
inner join person as person on x.person_id = person.id and person.record_state<>4
where ( x.record_state <> 4 )
