select 
    pr.id, 
    pr.caption,
    pr.caption as name,    
    per.user_id
  
from      sec_user_profile as up
join      org_employee         as emp on emp.id        = up.user_id
join      person               as per on emp.person_id = per.id
join      sec_profile          as pr  on pr.id         = up.profile_id
