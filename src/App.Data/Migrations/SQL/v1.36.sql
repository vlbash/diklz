
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191008093016_v1.36') THEN
    ALTER TABLE result_input_controls DROP CONSTRAINT fk_result_input_controls_iml_licenses_license_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191008093016_v1.36') THEN
    DROP INDEX ix_result_input_controls_license_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191008093016_v1.36') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191008093016_v1.36', '2.2.2-servicing-10034');
    END IF;
END $$;
