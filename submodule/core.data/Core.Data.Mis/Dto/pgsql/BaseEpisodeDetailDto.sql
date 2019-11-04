select
e.number,
e.id,
e.caption,
e.start_date,
e.end_date,
e.patient_card_id,
e.event_state_enum,
coalesce(doc.organization_id, uuid_in('00000000-0000-0000-0000-000000000000')) as organization_id,
(select caption from enum_record  where  enum_type = 'EpisodeState'  and code = e.event_state_enum) as event_state,
e.description,
e.doctor_id,
coalesce(person.last_name || ' ' || person.name|| ' ' ||   person.middle_name, '') as person_full_name,
coalesce(persdoc.last_name || ' ' || persdoc.name|| ' ' ||   persdoc.middle_name, '') as doctor_full_name,
coalesce(person.phone, '') as person_phone
from mis_episode e
left join mis_patient_card pc on e.patient_card_id = pc.id and pc.record_state<>4
left join person as person on  pc.person_id = person.id and person.record_state<>4
left join org_employee as doc on  e.doctor_id = doc.id and doc.record_state<>4
left join person as persdoc on  doc.person_id = persdoc.id and persdoc.record_state<>4
where e.record_state<>4
