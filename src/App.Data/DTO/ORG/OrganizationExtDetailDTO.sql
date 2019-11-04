select 
    --org.code,
    --org.parent_id,
    --org.legal_form_type,
    --org.ownership_type,
    --org.description,
    --org.phone_number,
    --org.fax_number,
    --org.post_index,
    --org.national_account,
    --org.international_account,
    --org.national_bank_requisites,
    --org.international_bank_requisites,
    --org.passport_serial,
    --org.passport_number,
    --org.passport_date,
    --org.passport_issue_unit,
    --org.email,
    org.id,
    coalesce(org.name, '') as name,
    coalesce(org.caption, '') as caption,
    coalesce(org.edrpou, '') as edrpou,
    coalesce(org.inn, '') as inn
from org_organization as org
where org.record_state <> 4