
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE iml_applications DROP CONSTRAINT "fk_iml_applications_departmental_subordinations_departmental_s~";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE prl_applications DROP CONSTRAINT "fk_prl_applications_departmental_subordinations_departmental_s~";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE trl_applications DROP CONSTRAINT "fk_trl_applications_departmental_subordinations_departmental_s~";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    DROP INDEX ix_trl_applications_departmental_subordination_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    DROP INDEX ix_prl_applications_departmental_subordination_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    DROP INDEX ix_iml_applications_departmental_subordination_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE trl_applications DROP COLUMN departmental_subordination_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE prl_applications DROP COLUMN departmental_subordination_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE iml_applications DROP COLUMN departmental_subordination_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE prl_applications ADD prl_in_pharmacies boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE prl_applications ADD retail_of_medicines boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE prl_applications ADD wholesale_of_medicines boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE iml_applications ADD prl_in_pharmacies boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE iml_applications ADD retail_of_medicines boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    ALTER TABLE iml_applications ADD wholesale_of_medicines boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190818081132_v1.22') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190818081132_v1.22', '2.2.2-servicing-10034');
    END IF;
END $$;
