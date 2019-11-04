
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926074304_v1.32') THEN
    CREATE TABLE pharmacy_item_pharmacies (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        pharmacy_id uuid NOT NULL,
        pharmacy_item_id uuid NOT NULL,
        CONSTRAINT pk_pharmacy_item_pharmacies PRIMARY KEY (id),
        CONSTRAINT fk_pharmacy_item_pharmacies_org_branches_pharmacy_id FOREIGN KEY (pharmacy_id) REFERENCES org_branches (id) ON DELETE CASCADE,
        CONSTRAINT fk_pharmacy_item_pharmacies_org_branches_pharmacy_item_id FOREIGN KEY (pharmacy_item_id) REFERENCES org_branches (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926074304_v1.32') THEN
    CREATE INDEX ix_pharmacy_item_pharmacies_pharmacy_id ON pharmacy_item_pharmacies (pharmacy_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926074304_v1.32') THEN
    CREATE INDEX ix_pharmacy_item_pharmacies_pharmacy_item_id ON pharmacy_item_pharmacies (pharmacy_item_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190926074304_v1.32') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190926074304_v1.32', '2.2.2-servicing-10034');
    END IF;
END $$;
