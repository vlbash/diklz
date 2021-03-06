select 
        coalesce(u.caption, '') as caption,
		u.id,
		coalesce(u.name, '') as name,
		p.full_name || ' ' || p.name || ' ' || p.middle_name as person_name,
		p.id as person_id,
         array_to_string(array(
                select ro.name
                FROM adm_user_adm_profiles AS rr
                	join adm_profiles as ro on rr.adm_profile_id=ro.id
                WHERE (rr.record_state <> 4) and (u.id = rr.adm_user_id)
        ), ', ') as profiles_info,
		array_to_string(array(
                select rr.adm_profile_id
                FROM adm_user_adm_profiles AS rr
                WHERE (rr.record_state <> 4) and (u.id = rr.adm_user_id)
        ), '|') AS profiles_string
from adm_users as u 
	join person as p on u.person_id=p.id
where u.record_state <> 4