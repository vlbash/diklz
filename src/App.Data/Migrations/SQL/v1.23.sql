
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190819162821_v1.23') THEN
    CREATE TABLE entity_enum_recordses (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        entity_id uuid NOT NULL,
        entity_type text NULL,
        enum_record_type text NULL,
        enum_record_code text NULL,
        CONSTRAINT pk_entity_enum_recordses PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190819162821_v1.23') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190819162821_v1.23', '2.2.2-servicing-10034');
    END IF;
END $$;
