select
	a.id,
	a.caption,
	a.start_date,
	a.end_date,
	a.patient_card_id,
    coalesce(d.organization_id, uuid_in('00000000-0000-0000-0000-000000000000')) as  organization_id,
	coalesce(pc.reg_number || ' (' || p.last_name || ' ' || p.name || ' ' || p.middle_name || ')', '') as patient_card_caption,
	coalesce(person.last_name || ' ' || person.name || ' ' || person.middle_name, '') as doctor_name,
	coalesce(s.caption, '') as doctor_speciality,
	coalesce(person.last_name || ' ' || substring(person.name from 1 for 1)|| '. ' || substring(person.middle_name from 1 for 1) || '. (' || s.caption || ')', '') as doctor_name_spec,
	(
		select caption
	from
		enum_record
	where
		enum_type = 'AppointmentState'
		and code = a.event_state_enum) as event_state
from
	mis_appointment a
left join mis_patient_card pc on
	pc.id = a.patient_card_id
	and pc.record_state <> 4
left join person p on
	p.id = pc.person_id
left join org_employee d on
	a.doctor_id = d.id
	and d.record_state <> 4
left join person as person on
	person.id = d.person_id
	and person.record_state <> 4
left join cdn_speciality as s on
	s.id = a.doctor_speciality_id
	and s.record_state <> 4
where
	a.record_state <> 4