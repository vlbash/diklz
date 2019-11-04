
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101144106_v1.41') THEN
    CREATE TABLE conclusion_medicines (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        application_id uuid NOT NULL,
        conclusion_application_id uuid NULL,
        medicine_name text NULL,
        form_name text NULL,
        dose_in_unit text NULL,
        number_of_units text NULL,
        medicine_name_eng text NULL,
        register_number text NULL,
        atc_code text NULL,
        producer_name text NULL,
        producer_country text NULL,
        supplier_name text NULL,
        supplier_country text NULL,
        supplier_address text NULL,
        is_from_license boolean NOT NULL,
        lims_rp_id uuid NOT NULL,
        notes text NULL,
        old_drug_id bigint NOT NULL,
        CONSTRAINT pk_conclusion_medicines PRIMARY KEY (id),
        CONSTRAINT "fk_conclusion_medicines_app_conclusions_conclusion_application~" FOREIGN KEY (conclusion_application_id) REFERENCES app_conclusions (id) ON DELETE RESTRICT,
        CONSTRAINT fk_conclusion_medicines_lims_rp_lims_rp_id FOREIGN KEY (lims_rp_id) REFERENCES lims_rp (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101144106_v1.41') THEN
    CREATE INDEX ix_conclusion_medicines_conclusion_application_id ON conclusion_medicines (conclusion_application_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101144106_v1.41') THEN
    CREATE INDEX ix_conclusion_medicines_lims_rp_id ON conclusion_medicines (lims_rp_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101144106_v1.41') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191101144106_v1.41', '2.2.2-servicing-10034');
    END IF;
END $$;
