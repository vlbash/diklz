select
upe.caption,
upe.id,
upe.org_unit_position_id,
op.name as org_unit_position_name,
op.code as org_position_code,
upe.employee_id,
pers.id as person_id,
pers.name as org_employee_name,
pers.middle_name as org_employee_middle_name,
pers.last_name as org_employee_full_name,
pers.last_name || ' ' || pers.name|| ' ' || pers.middle_name as org_employee_fio,
pers.last_name || ' ' || SUBSTRING(pers.name,1,1)  || '.' || SUBSTRING(pers.middle_name,1,1) || '.'  as org_employee_fioshort,
unit.id as org_unit_id,
unit.name as org_unit_name
from org_unit_position_employee as upe
inner join org_employee as oe on oe.id = upe.employee_id and oe.record_state<>4
inner join person as pers on pers.id = oe.person_id and pers.record_state<>4
inner join org_unit_position as oup on oup.id = upe.org_unit_position_id and oup.record_state<>4
inner join org_unit as unit on unit.id = oup.org_unit_id and unit.record_state<>4
inner join cdn_position as op on op.id = oup.position_id and op.record_state<>4