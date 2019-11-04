
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD base_class text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD description text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD old_lims_id bigint NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD org_unit_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD organization_info_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD parent_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD performer_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD reg_date timestamp without time zone NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD reg_number text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    CREATE INDEX ix_result_input_controls_org_unit_id ON result_input_controls (org_unit_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    CREATE INDEX ix_result_input_controls_parent_id ON result_input_controls (parent_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    CREATE INDEX ix_result_input_controls_performer_id ON result_input_controls (performer_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD CONSTRAINT fk_result_input_controls_org_organization_org_unit_id FOREIGN KEY (org_unit_id) REFERENCES org_organization (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD CONSTRAINT fk_result_input_controls_lims_docs_parent_id FOREIGN KEY (parent_id) REFERENCES lims_docs (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    ALTER TABLE result_input_controls ADD CONSTRAINT fk_result_input_controls_org_employee_performer_id FOREIGN KEY (performer_id) REFERENCES org_employee (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191001135811_v1.34') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191001135811_v1.34', '2.2.2-servicing-10034');
    END IF;
END $$;
