SELECT--coalesce(, '') as ,
brn.NAME,
coalesce(brn.phone_number, '') as phone_number,
COALESCE ( brn.caption, '' )                AS caption,
brn.organization_id,
apb.lims_document_id                        AS application_id,
apb.ID                                      AS application_branch_id,
coalesce(brn.fax_number, '') as fax_number,
coalesce(brn.email, '') as email,
COALESCE ( brn.adress_eng, '' )             AS adress_eng,
brn.prl_is_availiable_prod_sites AS prlis_availiable_prod_sites,
brn.prl_is_availiable_quality_zone AS prlis_availiable_quality_zone,
brn.prl_is_availiable_storage_zone AS prlis_availiable_storage_zone,
brn.prl_is_availiable_pickup_zone AS prlis_availiable_pickup_zone,
COALESCE ( brn.operation_list_form, '' ) AS operation_list_form,
COALESCE ( brn.operation_list_form_changing, '') as operation_list_form_changing,
brn.iml_is_availiable_storage_zone AS imlis_availiable_storage_zone,
brn.iml_is_availiable_permit_issue_zone AS imlis_availiable_permit_issue_zone,
brn.iml_is_availiable_quality AS imlis_availiable_quality,
brn.trl_is_manufacture AS trlis_manufacture,
brn.trl_is_wholesale AS trlis_wholesale,
brn.trl_is_retail AS trlis_retail,
brn.is_from_license,
brn.license_delete_check,
--ATU START
COALESCE ( brn.address_id, '00000000-0000-0000-0000-000000000000' ) AS address_id,
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
COALESCE ((SELECT NAME FROM atu_region AS reg WHERE SUBSTRING ( reg.code, 0, 6 ) = SUBSTRING ( city.code, 0, 6 ) LIMIT 1 ),'' ) AS district_name ,
--ATU END
brn.ID
	
FROM
	org_branches AS brn
	JOIN application_branches AS apb ON apb.branch_id = brn.ID 
	AND apb.record_state <> 4
	LEFT JOIN (
	SELECT
		imla.ID,
		imla.org_unit_id,
		imla.record_state,
		imla.app_type 
	FROM
		iml_applications AS imla UNION
	SELECT
		prla.ID,
		prla.org_unit_id,
		prla.record_state,
		prla.app_type 
	FROM
		prl_applications AS prla UNION
	SELECT
		trla.ID,
		trla.org_unit_id,
		trla.record_state,
		trla.app_type 
	FROM
		trl_applications AS trla 
	) app ON app.ID = apb.lims_document_id 
	AND app.record_state <> 4 
    --ATU START
	LEFT JOIN atu_subject_address AS subject_address ON brn.address_id = subject_address.ID
    LEFT JOIN atu_street AS street ON subject_address.street_id = street.ID
    LEFT JOIN atu_city AS city ON street.city_id = city.ID
    LEFT JOIN atu_region AS region ON city.region_id = region.ID 
    --ATU END
	
WHERE
	brn.record_state <> 4

