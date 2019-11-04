
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101112045_v1.40') THEN
    CREATE TABLE app_conclusions (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        parent_id uuid NULL,
        performer_id uuid NULL,
        reg_number text NULL,
        reg_date timestamp without time zone NULL,
        description text NULL,
        org_unit_id uuid NOT NULL,
        organization_info_id uuid NOT NULL,
        old_lims_id bigint NOT NULL,
        base_class text NULL,
        branch_id uuid NOT NULL,
        app_conclusion_status text NULL,
        app_state text NULL,
        app_sort text NULL,
        doc_num text NULL,
        app_reg_date timestamp without time zone NULL,
        assigne text NULL,
        teritorial_service text NULL,
        CONSTRAINT pk_app_conclusions PRIMARY KEY (id),
        CONSTRAINT fk_app_conclusions_org_branches_branch_id FOREIGN KEY (branch_id) REFERENCES org_branches (id) ON DELETE CASCADE,
        CONSTRAINT fk_app_conclusions_org_organization_org_unit_id FOREIGN KEY (org_unit_id) REFERENCES org_organization (id) ON DELETE CASCADE,
        CONSTRAINT fk_app_conclusions_lims_docs_parent_id FOREIGN KEY (parent_id) REFERENCES lims_docs (id) ON DELETE RESTRICT,
        CONSTRAINT fk_app_conclusions_org_employee_performer_id FOREIGN KEY (performer_id) REFERENCES org_employee (id) ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101112045_v1.40') THEN
    CREATE INDEX ix_app_conclusions_branch_id ON app_conclusions (branch_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101112045_v1.40') THEN
    CREATE INDEX ix_app_conclusions_org_unit_id ON app_conclusions (org_unit_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101112045_v1.40') THEN
    CREATE INDEX ix_app_conclusions_parent_id ON app_conclusions (parent_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101112045_v1.40') THEN
    CREATE INDEX ix_app_conclusions_performer_id ON app_conclusions (performer_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20191101112045_v1.40') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20191101112045_v1.40', '2.2.2-servicing-10034');
    END IF;
END $$;
