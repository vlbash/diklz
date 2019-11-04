
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    ALTER TABLE iml_medicines DROP CONSTRAINT fk_iml_medicines_atu_country_producer_country_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    ALTER TABLE iml_medicines DROP CONSTRAINT fk_iml_medicines_atu_country_supplier_country_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    DROP INDEX ix_iml_medicines_producer_country_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    DROP INDEX ix_iml_medicines_supplier_country_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    ALTER TABLE iml_medicines DROP COLUMN producer_country_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    ALTER TABLE iml_medicines DROP COLUMN supplier_country_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    ALTER TABLE iml_medicines ADD producer_country character varying(200) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    ALTER TABLE iml_medicines ADD supplier_country character varying(200) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726145407_v1.13') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190726145407_v1.13', '2.2.2-servicing-10034');
    END IF;
END $$;
