SELECT 
    qry.id,
    qry.caption,
    qry.modified_on,
    qry.org_unit_id,
    qry.app_type,
    qry.app_type_enum,
    qry.app_sort,
    qry.app_sort_enum,
    qry.app_state,
    qry.app_state_enum,
    qry.record_state,
    qry.prl_in_pharmacies,
    qry.retail_of_medicines,
    qry.wholesale_of_medicines
FROM
    (
    select
        imla.id,
        imla.caption,
        COALESCE(imla.modified_on, imla.created_on) as modified_on,
        imla.org_unit_id,
        type_enum.name as app_type,
        imla.app_type as app_type_enum,
        sort_enum.name as app_sort,
        imla.app_sort as app_sort_enum,
        state_enum.name as app_state,
        imla.app_state as app_state_enum,
        imla.record_state,
        imla.prl_in_pharmacies,
        imla.retail_of_medicines,
        imla.wholesale_of_medicines

    from iml_applications as imla
        left join enum_record as type_enum on type_enum.enum_type = 'ActivityType' and type_enum.code = imla.app_type
        left join enum_record as sort_enum on sort_enum.enum_type = 'ApplicationSort' and sort_enum.code = imla.app_sort
	    left join enum_record as state_enum on state_enum.enum_type = 'ApplicationState' and state_enum.code = imla.app_state



    UNION

    select
        prla.id,
        prla.caption,
        COALESCE(prla.modified_on, prla.created_on) as modified_on,
        prla.org_unit_id,
        type_enum.name as app_type,
        prla.app_type as app_type_enum,
        sort_enum.name as app_sort,
        prla.app_sort as app_sort_enum,
        state_enum.name as app_state,
        prla.app_state as app_state_enum,
        prla.record_state,
        prla.prl_in_pharmacies,
        prla.retail_of_medicines,
        prla.wholesale_of_medicines

    from prl_applications as prla
        left join enum_record as type_enum on type_enum.enum_type = 'ActivityType' and type_enum.code = prla.app_type
        left join enum_record as sort_enum on sort_enum.enum_type = 'ApplicationSort' and sort_enum.code = prla.app_sort
		left join enum_record as state_enum on state_enum.enum_type = 'ApplicationState' and state_enum.code = prla.app_state

    UNION

    select
        trla.id,
        trla.caption,
        COALESCE(trla.modified_on, trla.created_on) as modified_on,
        trla.org_unit_id,
        type_enum.name as app_type,
        trla.app_type as app_type_enum,
        sort_enum.name as app_sort,
        trla.app_sort as app_sort_enum,
        state_enum.name as app_state,
        trla.app_state as app_state_enum,
        trla.record_state,
        trla.prl_in_pharmacies,
        trla.retail_of_medicines,
        trla.wholesale_of_medicines

    from trl_applications as trla
        left join enum_record as type_enum on type_enum.enum_type = 'ActivityType' and type_enum.code = trla.app_type
        left join enum_record as sort_enum on sort_enum.enum_type = 'ApplicationSort' and sort_enum.code = trla.app_sort
		left join enum_record as state_enum on state_enum.enum_type = 'ApplicationState' and state_enum.code = trla.app_state
    ) qry

  where (true)
  AND qry.record_state <> 4