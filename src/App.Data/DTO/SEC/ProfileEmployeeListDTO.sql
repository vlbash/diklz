select 
    up.id, 
    up.caption, 
    up.user_id,
    up.profile_id, 
    pr.is_active,
    org.name as organization   
from      sec_user_profile as up
join      org_employee         as emp on emp.id        = up.user_id
join      sec_profile          as pr  on pr.id         = up.profile_id
left join org_organization     as org on org.id        = emp.organization_id

