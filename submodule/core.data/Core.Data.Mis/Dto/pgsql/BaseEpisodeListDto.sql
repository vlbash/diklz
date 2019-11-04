Select
e.number,
e.id,
e.caption,
e.start_date,
e.end_date,
e.event_state_enum,
(select caption from enum_record  where  enum_type = 'EpisodeState'  and code = e.event_state_enum) as event_state,
e.description,
coalesce(doc.organization_id, uuid_in('00000000-0000-0000-0000-000000000000')) as organization_id,
coalesce(doc.id, uuid_in('00000000-0000-0000-0000-000000000000')) as doctor_id,
pc.id as patient_card_id
from mis_episode e
inner join mis_patient_card pc on e.patient_card_id = pc.id and  pc.record_state<>4
inner join org_employee as doc on  e.doctor_id = doc.id and doc.record_state<>4
where e.record_state<>4