
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619145535_v1.02') THEN
    ALTER TABLE org_organization_info RENAME COLUMN is_pending_change_info TO is_pending_license_update;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190619145535_v1.02') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190619145535_v1.02', '2.2.2-servicing-10034');
    END IF;
END $$;
