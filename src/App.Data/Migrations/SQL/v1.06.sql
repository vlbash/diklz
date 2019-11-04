
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    ALTER TABLE trl_licenses ADD COLUMN order_date_holder timestamp without time zone NULL;
    UPDATE trl_licenses SET order_date_holder = order_date::timestamp;
    ALTER TABLE trl_licenses ALTER COLUMN order_date TYPE timestamp without time zone USING order_date_holder;
    ALTER TABLE trl_licenses DROP COLUMN order_date_holder;
    ALTER TABLE trl_licenses ALTER COLUMN order_date SET NOT NULL;
    ALTER TABLE trl_licenses ALTER COLUMN order_date DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    ALTER TABLE trl_licenses ADD COLUMN license_date_holder timestamp without time zone NULL;
    UPDATE trl_licenses SET license_date_holder = license_date::timestamp;
    ALTER TABLE trl_licenses ALTER COLUMN license_date TYPE timestamp without time zone USING license_date_holder;
    ALTER TABLE trl_licenses DROP COLUMN license_date_holder;
    ALTER TABLE trl_licenses ALTER COLUMN license_date SET NOT NULL;
    ALTER TABLE trl_licenses ALTER COLUMN license_date DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    ALTER TABLE prl_licenses ADD COLUMN order_date_holder timestamp without time zone NULL;
    UPDATE prl_licenses SET order_date_holder = order_date::timestamp;
    ALTER TABLE prl_licenses ALTER COLUMN order_date TYPE timestamp without time zone USING order_date_holder;
    ALTER TABLE prl_licenses DROP COLUMN order_date_holder;
    ALTER TABLE prl_licenses ALTER COLUMN order_date SET NOT NULL;
    ALTER TABLE prl_licenses ALTER COLUMN order_date DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    ALTER TABLE prl_licenses ADD COLUMN license_date_holder timestamp without time zone NULL;
    UPDATE prl_licenses SET license_date_holder = license_date::timestamp;
    ALTER TABLE prl_licenses ALTER COLUMN license_date TYPE timestamp without time zone USING license_date_holder;
    ALTER TABLE prl_licenses DROP COLUMN license_date_holder;
    ALTER TABLE prl_licenses ALTER COLUMN license_date SET NOT NULL;
    ALTER TABLE prl_licenses ALTER COLUMN license_date DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    ALTER TABLE iml_licenses ADD COLUMN order_date_holder timestamp without time zone NULL;
    UPDATE iml_licenses SET order_date_holder = order_date::timestamp;
    ALTER TABLE iml_licenses ALTER COLUMN order_date TYPE timestamp without time zone USING order_date_holder;
    ALTER TABLE iml_licenses DROP COLUMN order_date_holder;
    ALTER TABLE iml_licenses ALTER COLUMN order_date SET NOT NULL;
    ALTER TABLE iml_licenses ALTER COLUMN order_date DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    ALTER TABLE iml_licenses ADD COLUMN license_date_holder timestamp without time zone NULL;
    UPDATE iml_licenses SET license_date_holder = license_date::timestamp;
    ALTER TABLE iml_licenses ALTER COLUMN license_date TYPE timestamp without time zone USING license_date_holder;
    ALTER TABLE iml_licenses DROP COLUMN license_date_holder;
    ALTER TABLE iml_licenses ALTER COLUMN license_date SET NOT NULL;
    ALTER TABLE iml_licenses ALTER COLUMN license_date DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190711094043_v1.06') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190711094043_v1.06', '2.2.2-servicing-10034');
    END IF;
END $$;
