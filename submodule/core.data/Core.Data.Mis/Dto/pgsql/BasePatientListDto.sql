select 
p.id,
p.caption,
p.name,
p.middle_name,
p.last_name
from person as p 
join mis_patient_card pc on p.id = pc.person_id and pc.record_state <> 4
where p.record_state <> 4