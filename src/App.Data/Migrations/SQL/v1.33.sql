
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926135525_v1.33') THEN
    CREATE TABLE result_input_controls (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        state text NULL,
        teritorial_service text NULL,
        license_id uuid NOT NULL,
        lims_rpid uuid NOT NULL,
        register_number text NULL,
        end_date timestamp without time zone NULL,
        drug_name text NULL,
        drug_form text NULL,
        producer_name text NULL,
        producer_country text NULL,
        medicine_series text NULL,
        medicine_expiration_date timestamp without time zone NULL,
        size_of_series text NULL,
        unit_of_measurement text NULL,
        amount_of_imported_medicine text NULL,
        win_number text NULL,
        date_win timestamp without time zone NULL,
        input_control_result text NULL,
        name_of_mismatch text NULL,
        comment text NULL,
        CONSTRAINT pk_result_input_controls PRIMARY KEY (id),
        CONSTRAINT fk_result_input_controls_iml_licenses_license_id FOREIGN KEY (license_id) REFERENCES iml_licenses (id) ON DELETE CASCADE,
        CONSTRAINT fk_result_input_controls_lims_rp_lims_rpid FOREIGN KEY (lims_rpid) REFERENCES lims_rp (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926135525_v1.33') THEN
    CREATE INDEX ix_result_input_controls_license_id ON result_input_controls (license_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926135525_v1.33') THEN
    CREATE INDEX ix_result_input_controls_lims_rpid ON result_input_controls (lims_rpid);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926135525_v1.33') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190926135525_v1.33', '2.2.2-servicing-10034');
    END IF;
END $$;
