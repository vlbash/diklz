select
    aud.created_by ::uuid as id ,
	prop.new_value_formatted as entity_id,
    empl.organization_id as org_unit_id,
    aud.audit_entry_id,
    aud.entity_type_name as entity_type_name_code,
	enum_record."name" as entity_type_name,
    pers.last_name || ' ' || pers."name" || ' ' || pers.middle_name  as created_by,
    aud.created_date,
    aud.state,
    aud.state_name as caption
    from audit_entries aud
    join person pers on pers.id = aud.created_by ::uuid
    LEFT JOIN org_employee empl ON empl.person_id = aud.created_by ::uuid
		left join enum_record on enum_record.code = aud.entity_type_name
		left join audit_entry_properties prop on prop.audit_entry_id = aud.audit_entry_id and prop.property_name = 'Id'
    where aud.entity_type_name in ({0})