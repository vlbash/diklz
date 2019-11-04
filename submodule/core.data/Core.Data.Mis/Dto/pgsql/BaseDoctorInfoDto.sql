select
x.id,
x.caption,
x.person_id,
coalesce(es.speciality_id, uuid_in('00000000-0000-0000-0000-000000000000')) as main_speciality_id,
coalesce(spec.caption, '') as main_speciality_caption,
x.organization_id,
coalesce(person.last_name || ' ' || person.name|| ' ' ||   person.middle_name, '')  as person_full_name,
coalesce(org.caption, '') as organization_caption
from org_employee as x
  left join person as person on person.id = x.person_id and person.record_state <> 4
  left join org_organization as org on org.id = x.organization_id and org.record_state <> 4
  left join cdn_employee_speciality as es on (es.employee_id = x.id and es.is_main_speciality = true and es.record_state <> 4)
  left join cdn_speciality as spec on spec.id = es.speciality_id and spec.record_state <> 4
where x.discriminator = 'Doctor' and x.record_state <> 4