select
e.caption,
e.id,
p.last_name || ' ' || p.name || ' ' || p.middle_name  as name,
e.organization_id
from org_employee as e
join person as p on e.person_id=p.id and p.record_state<>4
where e.record_state<>4