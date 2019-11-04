
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    ALTER TABLE trl_applications ADD departmental_subordination_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    ALTER TABLE prl_applications ADD departmental_subordination_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    ALTER TABLE iml_applications ADD departmental_subordination_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    CREATE TABLE departmental_subordinations (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        name text NULL,
        code text NULL,
        CONSTRAINT pk_departmental_subordinations PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    CREATE INDEX ix_trl_applications_departmental_subordination_id ON trl_applications (departmental_subordination_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    CREATE INDEX ix_prl_applications_departmental_subordination_id ON prl_applications (departmental_subordination_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    CREATE INDEX ix_iml_applications_departmental_subordination_id ON iml_applications (departmental_subordination_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    ALTER TABLE iml_applications ADD CONSTRAINT "fk_iml_applications_departmental_subordinations_departmental_s~" FOREIGN KEY (departmental_subordination_id) REFERENCES departmental_subordinations (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    ALTER TABLE prl_applications ADD CONSTRAINT "fk_prl_applications_departmental_subordinations_departmental_s~" FOREIGN KEY (departmental_subordination_id) REFERENCES departmental_subordinations (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    ALTER TABLE trl_applications ADD CONSTRAINT "fk_trl_applications_departmental_subordinations_departmental_s~" FOREIGN KEY (departmental_subordination_id) REFERENCES departmental_subordinations (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190815144837_v1.19') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190815144837_v1.19', '2.2.2-servicing-10034');
    END IF;
END $$;
