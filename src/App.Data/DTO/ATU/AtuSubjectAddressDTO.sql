SELECT
	address.ID AS ID,
	street.ID AS street_id,
	street.NAME AS street_name,
	city.ID AS city_id,
	city.NAME AS city_name,
	city.type_enum AS city_enum,
	region.ID AS region_id,
	region.NAME AS region_name,
	address.post_index AS post_index,
	address.building AS building,
	address.address_type AS address_type,
    coalesce((select name from atu_region as reg where substring(reg.code,0,6) = substring(city.code,0,6) limit 1), '') as district_name,
	COALESCE ( address.caption, '' ) AS caption ,
    region.code::text as code
FROM
	atu_subject_address AS address
	LEFT JOIN atu_street AS street ON address.street_id = street.ID
    LEFT JOIN atu_city AS city ON street.city_id = city.ID
    LEFT JOIN atu_region AS region ON city.region_id = region.ID 
WHERE
	street.record_state <> 4