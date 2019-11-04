
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN supplier_name TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN supplier_name DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN supplier_name DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN supplier_country TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN supplier_country DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN supplier_country DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN register_number TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN register_number DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN register_number DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN producer_country TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN producer_country DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN producer_country DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN number_of_units TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN number_of_units DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN number_of_units DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN notes TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN notes DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN notes DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN medicine_name TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN medicine_name DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN medicine_name DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN dose_in_unit TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN dose_in_unit DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN dose_in_unit DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    ALTER TABLE iml_medicines ALTER COLUMN atc_code TYPE text;
    ALTER TABLE iml_medicines ALTER COLUMN atc_code DROP NOT NULL;
    ALTER TABLE iml_medicines ALTER COLUMN atc_code DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190801081342_v1.16') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190801081342_v1.16', '2.2.2-servicing-10034');
    END IF;
END $$;
