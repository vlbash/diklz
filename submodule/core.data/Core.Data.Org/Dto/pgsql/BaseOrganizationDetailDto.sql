with props as (select exp.code, entexp.value, entexp.entity_id
			   from ex_property exp
			   left join entity_ex_property entexp on entexp.ex_property_id = exp.id
	           where (exp.group = 'OrgUnit'))
select
 x.caption,
 x.id,
 coalesce(x.parent_id, uuid_in('00000000-0000-0000-0000-000000000000')) as parent_id,
 coalesce(x.description, '') as description,
 coalesce(x.state, '') as state,
 x.category as category,
 ( SELECT x0.caption
          FROM enum_record x0
         WHERE x0.enum_type = 'OrganizationCategory'::text AND lower(x0.code) = lower(x.category)
         LIMIT 1) AS category_caption,
coalesce(
( SELECT x1.caption
         FROM enum_record x1
         WHERE x1.enum_type = 'OrganizationStatus'::text AND lower(x1.code) = lower(x.state)
         LIMIT 1),
'')AS state_name,
coalesce(
(SELECT value FROM props WHERE (code = 'Phone') and (props.entity_id = x.id) LIMIT 1), '') AS phone,
coalesce(
(SELECT value FROM props WHERE (code = 'Email') and (props.entity_id = x.id) LIMIT 1), '') AS email,
coalesce(
				(select value
			from
				props
			where
				(code = 'EDRPOU')
				and (props.entity_id = x.id)
			limit 1), '')::text as edrpou,
coalesce(
(SELECT value FROM props WHERE (code = 'BankDetails') and (props.entity_id = x.id) LIMIT 1), '') AS bank_details,
coalesce(
(SELECT value FROM props WHERE (code = 'FullName') and (props.entity_id = x.id) LIMIT 1), '') AS full_name,
 ( select    case when ua.post_index <> '' then ua.post_index || ', ' else '' end ||
              case when c.caption <> '' then c.caption || '  ' else '' end ||
              case when s.caption <> '' then s.caption || ',  ' else '' end ||
              case when ua.building <> '' then ua.building else '' end) as  jur_address,
coalesce(country.id, uuid_in('00000000-0000-0000-0000-000000000000')) as atu_country_id,
coalesce(
(case when region.id is not null then  region.id else f.id end), uuid_in('00000000-0000-0000-0000-000000000000')) as atu_region_id,
coalesce(f.id, uuid_in('00000000-0000-0000-0000-000000000000')) as atu_region_district_id,
coalesce(c.id, uuid_in('00000000-0000-0000-0000-000000000000')) as atu_city_id,
coalesce(s.id, uuid_in('00000000-0000-0000-0000-000000000000')) as atu_street_id,
coalesce(s.caption, '') as atu_street_caption,
coalesce(ua.id, uuid_in('00000000-0000-0000-0000-000000000000')) as subject_atu_address_id,
coalesce(ua.post_index, '') as post_index,
coalesce(ua.address_type, '') as address_type,
coalesce(ua.building, '') as building,
coalesce(parent.caption, '') as parent
from org_organization as x
    left join org_unit as parent on parent.id=x.parent_id and parent.record_state<>4
    left join atu_subject_address ua on ua.address_type = 'Juridical' and ua.subject_id= x.id and ua.record_state <> 4
    left join atu_street s on s.id = ua.street_id and s.record_state<>4
    left join atu_city as c on c.id = s.city_id and c.record_state<>4
    left join atu_region as f on c.region_id = f.id and f.record_state<>4
    left join atu_region as region on region.id = f.parent_id and f.record_state<>4
    left join atu_country as country on f.country_id = country.id and country.record_state<>4
where ( x.record_state <> 4 )