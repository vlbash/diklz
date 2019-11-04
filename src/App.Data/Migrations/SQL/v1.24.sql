
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE org_branches DROP COLUMN iml_another_activity;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE org_branches DROP COLUMN iml_is_importing_finished;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE org_branches DROP COLUMN iml_is_importing_in_bulk;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE iml_applications ADD iml_another_activity character varying(255) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE iml_applications ADD iml_is_importing_finished boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE iml_applications ADD iml_is_importing_in_bulk boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE iml_applications ADD is_conditions_for_control boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    ALTER TABLE iml_applications ADD is_good_manufacturing_practice boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190820121441_v1.24') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190820121441_v1.24', '2.2.2-servicing-10034');
    END IF;
END $$;
