SELECT                      
	org_info.id as Id,
	org_info.name                       as org_name,
    org_info.id                         as org_unit_id,
    coalesce(org_info.caption, '')      as caption

from
	org_organization as org_info