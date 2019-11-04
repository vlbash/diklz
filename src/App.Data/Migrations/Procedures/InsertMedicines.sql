        CREATE OR REPLACE PROCEDURE InsertMedicines(p1 TEXT)
        LANGUAGE plpgsql
        AS $$
        DECLARE
            BEGIN

             INSERT into iml_medicines(id, application_id, record_state, caption, modified_by, created_by, created_on, medicine_name, form_name, dose_in_unit, number_of_units,
            medicine_name_eng, register_number, atc_code, producer_name, producer_country, supplier_name, supplier_country, supplier_address, is_from_license,
            lims_rp_id, notes, old_drug_id)

        SELECT uuid_in(md5(random()::text || clock_timestamp()::text)::cstring), uuid(elem->>'ApplicationId'), 2, '', uuid(elem->>'CreatedByJson'), uuid(elem->>'CreatedByJson'), NOW(), elem->>'MedicineName', elem->>'FormName', elem->>'DoseInUnit', elem->>'NumberOfUnits', elem->>'MedicineNameEng', elem->>'RegisterNumber', elem->>'AtcCode', elem->>'ProducerName',
        elem->>'ProducerCountry', elem->>'SupplierName', elem->>'SupplierCountry', elem->>'SupplierAddress', bool(elem->>'IsFromLicense'), uuid(elem->>'LimsRpId'),
        elem->>'Notes', NULLIF(elem->>'OLdDRugId', '')::int
        FROM json_array_elements(p1::json) as elem;
            END; 
            $$;