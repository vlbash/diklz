select
        x.caption,
        x.id,
        x.name,
        x.is_active,
        array_to_string(array(
                select rr.adm_role_id
                FROM adm_profile_adm_roles AS rr
                WHERE (rr.record_state <> 4) and (x.id = rr.adm_profile_id)
        ), '|') AS roles_string,
         array_to_string(array(
                select ro.name
                FROM adm_profile_adm_roles AS rr
                	join adm_roles as ro on rr.adm_role_id=ro.id
                WHERE (rr.record_state <> 4) and (x.id = rr.adm_profile_id)
        ), ', ') as roles_info,
		array_to_string(array(
                select rr.atu_region_id
                FROM adm_profile_atu_regions AS rr
                WHERE (rr.record_state <> 4) and (x.id = rr.adm_profile_id)
        ), '|') AS regions_string,
        array_to_string(array(
                select ro.name
                FROM adm_profile_atu_regions AS rr
                	join atu_region as ro on rr.atu_region_id=ro.id
                WHERE (rr.record_state <> 4) and (x.id = rr.adm_profile_id)
        ), ', ') as regions_info
    from adm_profiles x
	where x.record_state <> 4