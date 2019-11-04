
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719134258_v1.10') THEN
    ALTER TABLE iml_medicines ADD application_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719134258_v1.10') THEN
    ALTER TABLE iml_medicines ADD iml_application_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719134258_v1.10') THEN
    CREATE INDEX ix_iml_medicines_iml_application_id ON iml_medicines (iml_application_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719134258_v1.10') THEN
    ALTER TABLE iml_medicines ADD CONSTRAINT fk_iml_medicines_iml_applications_iml_application_id FOREIGN KEY (iml_application_id) REFERENCES iml_applications (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190719134258_v1.10') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190719134258_v1.10', '2.2.2-servicing-10034');
    END IF;
END $$;
