
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190809125322_v1.17') THEN
    ALTER TABLE iml_medicines ADD is_from_license boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190809125322_v1.17') THEN
    CREATE INDEX ix_iml_medicines_application_id ON iml_medicines (application_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190809125322_v1.17') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190809125322_v1.17', '2.2.2-servicing-10034');
    END IF;
END $$;
