SELECT
city.id as id,
city.name as name,
city.type_enum,
concat(city.name,' ',city.code) as name_code,
city.code,
region.id as region_id,
region.name as region_name,
coalesce((select name from atu_region as reg where substring(reg.code,0,6) = substring(city.code,0,6) limit 1), '') as district_name,
coalesce(city.caption, '') as caption
FROM atu_city as city 
LEFT JOIN atu_region as region on city.region_id = region.id
WHERE city.record_state <> 4 and region.record_state <> 4 {0}