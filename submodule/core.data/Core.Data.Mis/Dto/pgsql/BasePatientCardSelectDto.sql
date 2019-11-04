select 
pc.id,
pc.reg_number || ' (' || p.last_name || ' ' || p.name || ' ' || p.middle_name || ')' as caption
from person p
	join mis_patient_card pc on p.id = pc.person_id and pc.record_state <> 4
where p.record_state <> 4