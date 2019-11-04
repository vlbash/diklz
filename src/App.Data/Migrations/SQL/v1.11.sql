
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    ALTER TABLE iml_medicines DROP COLUMN producer_country;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    ALTER TABLE iml_medicines DROP COLUMN supplier_country;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    ALTER TABLE iml_medicines ADD producer_country_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    ALTER TABLE iml_medicines ADD supplier_country_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    CREATE INDEX ix_iml_medicines_producer_country_id ON iml_medicines (producer_country_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    CREATE INDEX ix_iml_medicines_supplier_country_id ON iml_medicines (supplier_country_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    ALTER TABLE iml_medicines ADD CONSTRAINT fk_iml_medicines_atu_country_producer_country_id FOREIGN KEY (producer_country_id) REFERENCES atu_country (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    ALTER TABLE iml_medicines ADD CONSTRAINT fk_iml_medicines_atu_country_supplier_country_id FOREIGN KEY (supplier_country_id) REFERENCES atu_country (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190722134338_v1.11') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190722134338_v1.11', '2.2.2-servicing-10034');
    END IF;
END $$;
