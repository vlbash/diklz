select 
    emp.id,
    emp.person_id,
    
    coalesce(person.last_name || ' ' || person.name|| ' ' ||   person.middle_name,'') as person_fio,
    
    coalesce(person.last_name,'')                    as person_last_name,
    coalesce(person.name ,'')                        as person_name,
    coalesce(person.middle_name   ,'')               as person_middle_name,
    
    coalesce(person.ipn ,'')                         as person_ipn,
    coalesce(person.phone  ,'')                      as person_phone,

    coalesce(unitpos.unit_id, '00000000-0000-0000-0000-000000000000')  as org_unit_id,
    coalesce(unitpos.unit_name ,'')                  as org_unit,
    
    coalesce(emp.user_email  ,'')                    as user_email,
    
    coalesce(unitpos.position_name ,'')              as org_unit_position,
    coalesce(unitpos.org_unit_position_id, '00000000-0000-0000-0000-000000000000') as org_unit_position_id,
    
    coalesce(emp.receive_on_change_all_application, false)     as receive_on_change_all_application,
    coalesce(emp.receive_on_change_all_message ,false)        as receive_on_change_all_message,
    coalesce(emp.receive_on_change_own_application ,false)     as receive_on_change_own_application,
    coalesce(emp.receive_on_change_own_message ,false)         as receive_on_change_own_message,
    coalesce(emp.personal_cabinet_status    ,false)            as personal_cabinet_status,
    coalesce(emp.caption,'') as caption,
    coalesce(emp.old_lims_id,0) as old_lims_id

from org_employee as emp
inner join person on emp.person_id = person.id and person.record_state <> 4
left join (select 
               upe.employee_id,
              p.name as position_name,
              up.id as position_id,
              unit.name as unit_name,
              unit.id as unit_id,
              upe.org_unit_position_id as org_unit_position_id
              from org_unit_position_employee as upe
              inner join org_unit_position as up on upe.org_unit_position_id = up.id and up.record_state <> 4
              inner join cdn_position as p on up.position_id = p.id and p.record_state <> 4
              inner join org_unit as unit on up.org_unit_id = unit.id and unit.record_state <> 4)
               as unitpos on unitpos.employee_id = emp.id
               
               
where (emp.record_state <> 4)