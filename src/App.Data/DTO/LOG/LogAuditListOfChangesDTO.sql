select
('00000000-0000-0000-0000-000000000000') ::uuid as id,
aud.audit_entry_id,
aud.property_name,
aud.property_name as caption,
aud.new_value_formatted,
aud.old_value_formatted,
aud_entr.entity_type_name as entity_name
from audit_entry_properties aud
left join audit_entries aud_entr on aud_entr.audit_entry_id = aud.audit_entry_id