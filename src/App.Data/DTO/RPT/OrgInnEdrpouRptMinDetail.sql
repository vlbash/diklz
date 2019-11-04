SELECT                      
	org.id                                  as Id,
    coalesce(org.caption, '')               as caption,
    org.inn,
    org.edrpou

from
	org_organization as org

where
    org.record_state <> 4