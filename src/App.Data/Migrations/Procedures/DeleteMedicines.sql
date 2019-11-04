        CREATE OR REPLACE procedure DeleteMedicines(p1 uuid, p2 BOOLEAN)
        LANGUAGE plpgsql
        AS $$
        DECLARE
            BEGIN

             delete from iml_medicines
						 where application_id = p1 and is_from_license = p2;
						 END; 
            $$;
						