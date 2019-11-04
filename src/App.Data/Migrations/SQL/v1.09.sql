
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719124816_v1.09') THEN
    CREATE TABLE iml_medicines (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        medicine_name character varying(200) NULL,
        form_name text NULL,
        dose_in_unit character varying(200) NULL,
        number_of_units character varying(100) NULL,
        medicine_name_eng text NULL,
        register_number character varying(200) NULL,
        atc_code character varying(100) NULL,
        producer_name character varying(200) NULL,
        producer_country character varying(200) NULL,
        supplier_name character varying(200) NULL,
        supplier_country character varying(200) NULL,
        supplier_address text NULL,
        notes character varying(1000) NULL,
        CONSTRAINT pk_iml_medicines PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719124816_v1.09') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190719124816_v1.09', '2.2.2-servicing-10034');
    END IF;
END $$;
