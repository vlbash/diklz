select
a.id,
a.caption,
a.start_date,
a.end_date,
a.signature_date,
a.patient_card_id,
a.doctor_id,
coalesce(d.organization_id, uuid_in('00000000-0000-0000-0000-000000000000')) as organization_id,
coalesce(doctor.last_name || ' ' || doctor.name|| ' ' ||   doctor.middle_name || ' (' || s.caption || ')', '') as doctor_name,
coalesce(s.caption, '') as doctor_speciality_name,
a.doctor_speciality_id,
a.is_first,
a.event_state_enum,
a.interaction_type_enum,
(select caption from enum_record  where  enum_type = 'AppointmentState'  and code = a.event_state_enum) as event_state,
(select caption from enum_record  where  enum_type = 'InteractionType'  and code = a.interaction_type_enum) as interaction_type,
a.doctor_recomendation,
a.complaints,
a.description,
coalesce(patient.last_name || ' ' || patient.name|| ' ' ||   patient.middle_name, '')  as person_full_name,
coalesce(pc.reg_number || ' (' || patient.last_name || ' ' || patient.name || ' ' || patient.middle_name || ')', '') as patient_card_caption,
coalesce(patient.phone, '') as person_phone
from mis_appointment a
inner join mis_patient_card pc on pc.id = a.patient_card_id and pc.record_state<>4
inner join org_employee d on a.doctor_id = d.id and d.record_state<>4
inner join person as doctor on doctor.id = d.person_id and doctor.record_state<>4
inner join cdn_speciality as s on s.id = a.doctor_speciality_id and s.record_state<>4
inner join person as patient on  pc.person_id = patient.id  and pc.record_state<>4
where a.record_state <> 4