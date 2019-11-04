
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190816113841_v1.20') THEN
    ALTER TABLE org_organization_info ADD economic_classification_type character varying(255) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190816113841_v1.20') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190816113841_v1.20', '2.2.2-servicing-10034');
    END IF;
END $$;
