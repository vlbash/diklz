SELECT--coalesce(, '') as ,
vie.ID,
vie.license_number,
vie.license_date,
vie.order_date,
vie.order_number,
vie.caption,
vie.org_director,
vie.org_unit_id 
FROM
	(
	SELECT
		lic.ID AS ID,
		lic.license_number,
		lic.license_date,
		lic.order_date,
		lic.order_number,
		COALESCE ( app.caption, '' ) AS caption,
		COALESCE ( ori.org_director, '' ) AS org_director,
		COALESCE ( app.org_unit_id, '00000000-0000-0000-0000-000000000000' ) AS org_unit_id 
	FROM
		prl_licenses AS lic
		LEFT JOIN prl_applications AS app ON app.ID = lic.parent_id
		LEFT JOIN org_organization_info AS ori ON app.organization_info_id = ori.ID AND ori.record_state <> 4 
		
		UNION
		
	SELECT
		lic.ID AS ID,
		lic.license_number,
		lic.license_date,
		lic.order_date,
		lic.order_number,
		COALESCE ( app.caption, '' ) AS caption,
		COALESCE ( ori.org_director, '' ) AS org_director,
		COALESCE ( app.org_unit_id, '00000000-0000-0000-0000-000000000000' ) AS org_unit_id 
	FROM
		iml_licenses AS lic
		LEFT JOIN iml_applications AS app ON app.ID = lic.parent_id
		LEFT JOIN org_organization_info AS ori ON app.organization_info_id = ori.ID AND ori.record_state <> 4 
		
		UNION
		
	SELECT
		lic.ID AS ID,
		lic.license_number,
		lic.license_date,
		lic.order_date,
		lic.order_number,
		COALESCE ( app.caption, '' ) AS caption,
		COALESCE ( ori.org_director, '' ) AS org_director,
		COALESCE ( app.org_unit_id, '00000000-0000-0000-0000-000000000000' ) AS org_unit_id 
	FROM
		trl_licenses AS lic
		LEFT JOIN trl_applications AS app ON app.ID = lic.parent_id
		LEFT JOIN org_organization_info AS ori ON app.organization_info_id = ori.ID AND ori.record_state <> 4 
	) vie