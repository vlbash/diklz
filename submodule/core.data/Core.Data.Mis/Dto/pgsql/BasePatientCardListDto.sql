select 
pc.id,
pc.enabled,
pc.description,
pc.caption,
pc.reg_date,
pc.reg_number,
p.last_name || ' ' || p.name || ' ' || p.middle_name as person_full_name,
p.birthday,
p.email,
p.phone,
p.gender_enum as gender_enum,
(select name from enum_record  where  enum_type = 'Gender'  and code = p.gender_enum) as gender
from person p
	join mis_patient_card pc on p.id = pc.person_id and pc.record_state <> 4
where p.record_state <> 4