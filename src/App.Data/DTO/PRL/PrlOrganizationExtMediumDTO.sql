select
    org.id,
    coalesce(ooi.name, '') as name,
    coalesce(org.edrpou,'') as edrpou,
    coalesce(org.inn,'') as inn,
    coalesce(ooi.email,'') as email,
    coalesce(org.caption,'') as caption,
    coalesce(ooi.org_director,'') as org_director,
    --ATU START
    COALESCE ( ooi.address_id, '00000000-0000-0000-0000-000000000000' ) AS address_id,
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
--ATU END
from org_organization as org
join org_organization_info ooi
    on ooi.organization_id = org.id
    and ooi.is_actual_info = true
    and ooi.type = 'PRL'
--ATU START
	LEFT JOIN atu_subject_address AS subject_address ON  subject_address.ID = ooi.address_id
    LEFT JOIN atu_street AS street ON subject_address.street_id = street.ID
    LEFT JOIN atu_city AS city ON street.city_id = city.ID
    LEFT JOIN atu_region AS region ON city.region_id = region.ID 
--ATU END

where org.record_state <> 4