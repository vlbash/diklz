select
x.id,
x.caption,
x.person_id,
x.treats_children,
x.organization_id,
coalesce(es.speciality_id, uuid_in('00000000-0000-0000-0000-000000000000')) as main_speciality_id,
coalesce(spec.caption, '') as main_speciality_caption,
coalesce(person.last_name || ' ' || person.name|| ' ' ||   person.middle_name, '') as person_full_name,
coalesce(person.last_name, '') as person_last_name,
coalesce(person.name, '') as person_name,
coalesce(person.middle_name, '') as person_middle_name,
coalesce(person.phone, '') as person_phone,
coalesce(person.email, '') as person_email,
coalesce(org.caption, '') as organization_caption
from org_employee as x
  inner join person as person on person.id = x.person_id and person.record_state <> 4
  left join org_organization as org on org.id = x.organization_id and org.record_state <> 4
  left join cdn_employee_speciality as es on (es.employee_id = x.id and es.is_main_speciality = true and es.record_state <> 4)
  left join cdn_speciality as spec on spec.id = es.speciality_id and spec.record_state <> 4
where x.discriminator = 'Doctor' and x.record_state <> 4