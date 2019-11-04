SELECT                      
	org_branches.id                         as Id,
    org_branches.organization_id            as org_unit_id,
	coalesce(org_branches.name, '')         as mpd_name,
    coalesce(org_branches.caption, '')      as caption

from
	org_branches

where
    org_branches.record_state <> 4