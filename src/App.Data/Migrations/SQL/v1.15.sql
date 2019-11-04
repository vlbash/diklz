
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190731152026_v1.15') THEN
    ALTER TABLE iml_medicines ALTER COLUMN producer_name TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN producer_name DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN producer_name DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190731152026_v1.15') THEN
    CREATE INDEX ix_lims_rp_end_date ON lims_rp (end_date);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190731152026_v1.15') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190731152026_v1.15', '2.2.2-servicing-10034');
    END IF;
END $$;
