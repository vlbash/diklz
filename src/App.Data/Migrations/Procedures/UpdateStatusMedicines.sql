        CREATE OR REPLACE procedure UpdateStatusMedicines(p1 uuid)
        LANGUAGE plpgsql
        AS $$
        DECLARE
            BEGIN

             update iml_medicines
						 set is_from_license = true
						 where application_id = p1;
						 END; 
            $$;