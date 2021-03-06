SELECT distinct
    ass.id,
    ass."name", 
    ass.middle_name, 
    ass.last_name, 
    ass.name_of_position, 
    ass.date_of_appointment, 
    ass.caption,
    ass.is_from_license,
    brn.organization_id as org_unit_id,
    ass.record_state,
    type.name as assigne_type_name
FROM (select  * from app_assignees where record_state !=4) as ass
    join app_assignee_branches as brass on brass.assignee_id = ass.id and brass.record_state <> 4
    JOIN org_branches as brn on brn.id = brass.branch_id and brn.record_state <> 4
    JOIN application_branches as apb on apb.branch_id = brn.id and apb.record_state <> 4
    LEFT JOIN enum_record as type on type.code = ass.org_position_type
WHERE apb.lims_document_id in ({0})