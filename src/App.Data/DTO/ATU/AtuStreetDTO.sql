SELECT
street.id as id,
street.name as name,
city.id as city_id,
city.name as city_name,
street.caption as type_enum,
coalesce(street.caption, '') as caption
FROM atu_street as street 
LEFT JOIN atu_city as city on street.city_id = city.id
WHERE street.record_state <> 4