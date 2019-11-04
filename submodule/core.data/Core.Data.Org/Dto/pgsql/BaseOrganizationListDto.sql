with props as (select exp.code, entexp.value, entexp.entity_id
			   from ex_property exp
			   left join entity_ex_property entexp on entexp.ex_property_id = exp.id
	           where (exp.group = 'OrgUnit'))
select
 x.caption,
 x.id,
 x.state as state,
 x.category as category,
 ( SELECT x0.caption
          FROM enum_record x0
         WHERE x0.enum_type = 'OrganizationCategory'::text AND lower(x0.code) = lower(x.category)
         LIMIT 1) AS category_caption,
( SELECT x1.caption
         FROM enum_record x1
         WHERE x1.enum_type = 'OrganizationStatus'::text AND lower(x1.code) = lower(x.state)
         LIMIT 1) AS state_caption,
(SELECT value FROM props WHERE (code = 'Phone') and (props.entity_id = x.id) LIMIT 1) AS phone,
(SELECT value FROM props WHERE (code = 'Email') and (props.entity_id = x.id) LIMIT 1) AS email,
coalesce(
				(select value
			from
				props
			where
				(code = 'EDRPOU')
				and (props.entity_id = x.id)
			limit 1), '')::text as edrpou,
 (addr1.region || ' область') as region,
 addr1.region_id,
 addr1.address,
 o2.caption as parent
from
 org_organization as x
left join ( select ua.id, ua.subject_id,
              case when ua.post_index <> '' then ua.post_index || ', ' else '' end ||
              case when c.caption <> '' then c.caption || ' ' else '' end ||
              case when s.caption <> '' then s.caption || ', ' else '' end ||
              case when ua.building <> '' then ua.building else '' end
		            as address,
              (case when region.caption <> '' then  region.caption else f.caption end) as region,
			  (case when region.id is not null then  region.id else f.id end) as region_id
   from atu_subject_address as ua
   inner join atu_street s on s.id = ua.street_id and s.record_state<>4
   inner join atu_city as c on c.id = s.city_id and c.record_state<>4
   inner join atu_region as f on c.region_id = f.id and f.record_state<>4
   left  join atu_region as region on region.id = f.parent_id and f.record_state<>4
   where (ua.record_state <> 4) and (ua.address_type = 'Juridical'))
     as addr1 on addr1.subject_id = x.id
left join org_organization as o2 on o2.id=x.parent_id
where ( x.record_state <> 4 )