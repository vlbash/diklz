SELECT
	vie.ID,
	vie.license_number,
	vie.license_date,
	vie.order_date,
	vie.order_number,
	vie.caption,
	vie.org_unit_id,
	vie.address_id,
	vie.street_id,
	vie.street_name,
	vie.city_id,
	vie.city_name,
	vie.city_enum,
	vie.region_id,
	vie.region_name,
	vie.post_index,
	vie.building,
	vie.address_type,
	vie.district_name 
FROM
	(
	SELECT
		lic.ID AS ID,
		lic.license_number,
		lic.license_date,
		lic.order_date,
		lic.order_number,
		COALESCE ( app.caption, '' ) AS caption,
		COALESCE ( app.org_unit_id, '00000000-0000-0000-0000-000000000000' ) AS org_unit_id,
		COALESCE ( ori.address_id, '00000000-0000-0000-0000-000000000000' ) AS address_id,
		COALESCE ( street.ID, '00000000-0000-0000-0000-000000000000' ) AS street_id,
		COALESCE ( street.NAME, '' ) AS street_name,
		COALESCE ( city.ID, '00000000-0000-0000-0000-000000000000' ) AS city_id,
		COALESCE ( city.NAME, '' ) AS city_name,
		COALESCE ( city.type_enum, '' ) AS city_enum,
		COALESCE ( region.ID, '00000000-0000-0000-0000-000000000000' ) AS region_id,
		COALESCE ( region.NAME, '' ) AS region_name,
		COALESCE ( subject_address.post_index, '' ) AS post_index,
		COALESCE ( subject_address.building, '' ) AS building,
		COALESCE ( subject_address.address_type, '' ) AS address_type,
		COALESCE ((SELECT NAME FROM atu_region AS reg WHERE SUBSTRING ( reg.code, 0, 6 ) = SUBSTRING ( city.code, 0, 6 ) LIMIT 1 ),'' ) AS district_name 
	FROM
		prl_licenses AS lic
		LEFT JOIN prl_applications AS app ON app.ID = lic.parent_id
		LEFT JOIN org_organization_info AS ori ON app.organization_info_id = ori.ID AND ori.record_state <> 4
		LEFT JOIN atu_subject_address AS subject_address ON subject_address.ID = ori.address_id
		LEFT JOIN atu_street AS street ON subject_address.street_id = street.ID LEFT JOIN atu_city AS city ON street.city_id = city.ID LEFT JOIN atu_region AS region ON city.region_id = region.ID 
		
	UNION
		
	SELECT
		lic.ID AS ID,
		lic.license_number,
		lic.license_date,
		lic.order_date,
		lic.order_number,
		COALESCE ( app.caption, '' ) AS caption,
		COALESCE ( app.org_unit_id, '00000000-0000-0000-0000-000000000000' ) AS org_unit_id,
		COALESCE ( ori.address_id, '00000000-0000-0000-0000-000000000000' ) AS address_id,
		COALESCE ( street.ID, '00000000-0000-0000-0000-000000000000' ) AS street_id,
		COALESCE ( street.NAME, '' ) AS street_name,
		COALESCE ( city.ID, '00000000-0000-0000-0000-000000000000' ) AS city_id,
		COALESCE ( city.NAME, '' ) AS city_name,
		COALESCE ( city.type_enum, '' ) AS city_enum,
		COALESCE ( region.ID, '00000000-0000-0000-0000-000000000000' ) AS region_id,
		COALESCE ( region.NAME, '' ) AS region_name,
		COALESCE ( subject_address.post_index, '' ) AS post_index,
		COALESCE ( subject_address.building, '' ) AS building,
		COALESCE ( subject_address.address_type, '' ) AS address_type,
		COALESCE ((SELECT NAME FROM atu_region AS reg WHERE SUBSTRING ( reg.code, 0, 6 ) = SUBSTRING ( city.code, 0, 6 ) LIMIT 1 ),'' ) AS district_name 
	FROM
		iml_licenses AS lic
		LEFT JOIN iml_applications AS app ON app.ID = lic.parent_id
		LEFT JOIN org_organization_info AS ori ON app.organization_info_id = ori.ID AND ori.record_state <> 4
		LEFT JOIN atu_subject_address AS subject_address ON subject_address.ID = ori.address_id
		LEFT JOIN atu_street AS street ON subject_address.street_id = street.ID LEFT JOIN atu_city AS city ON street.city_id = city.ID LEFT JOIN atu_region AS region ON city.region_id = region.ID 
		
		UNION
		
	SELECT
		lic.ID AS ID,
		lic.license_number,
		lic.license_date,
		lic.order_date,
		lic.order_number,
		COALESCE ( app.caption, '' ) AS caption,
		COALESCE ( app.org_unit_id, '00000000-0000-0000-0000-000000000000' ) AS org_unit_id,
		COALESCE ( ori.address_id, '00000000-0000-0000-0000-000000000000' ) AS address_id,
		COALESCE ( street.ID, '00000000-0000-0000-0000-000000000000' ) AS street_id,
		COALESCE ( street.NAME, '' ) AS street_name,
		COALESCE ( city.ID, '00000000-0000-0000-0000-000000000000' ) AS city_id,
		COALESCE ( city.NAME, '' ) AS city_name,
		COALESCE ( city.type_enum, '' ) AS city_enum,
		COALESCE ( region.ID, '00000000-0000-0000-0000-000000000000' ) AS region_id,
		COALESCE ( region.NAME, '' ) AS region_name,
		COALESCE ( subject_address.post_index, '' ) AS post_index,
		COALESCE ( subject_address.building, '' ) AS building,
		COALESCE ( subject_address.address_type, '' ) AS address_type,
		COALESCE ((SELECT NAME FROM atu_region AS reg WHERE SUBSTRING ( reg.code, 0, 6 ) = SUBSTRING ( city.code, 0, 6 ) LIMIT 1 ),'' ) AS district_name 
	FROM
		trl_licenses AS lic
		LEFT JOIN trl_applications AS app ON app.ID = lic.parent_id
		LEFT JOIN org_organization_info AS ori ON app.organization_info_id = ori.ID AND ori.record_state <> 4
		LEFT JOIN atu_subject_address AS subject_address ON subject_address.ID = ori.address_id
		LEFT JOIN atu_street AS street ON subject_address.street_id = street.ID LEFT JOIN atu_city AS city ON street.city_id = city.ID LEFT JOIN atu_region AS region ON city.region_id = region.ID 
	) vie