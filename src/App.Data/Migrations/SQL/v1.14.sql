
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726153049_v1.14') THEN
    ALTER TABLE iml_medicines ADD lims_rp_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726153049_v1.14') THEN
    CREATE INDEX ix_iml_medicines_lims_rp_id ON iml_medicines (lims_rp_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726153049_v1.14') THEN
    ALTER TABLE iml_medicines ADD CONSTRAINT fk_iml_medicines_lims_rp_lims_rp_id FOREIGN KEY (lims_rp_id) REFERENCES lims_rp (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190726153049_v1.14') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190726153049_v1.14', '2.2.2-servicing-10034');
    END IF;
END $$;
