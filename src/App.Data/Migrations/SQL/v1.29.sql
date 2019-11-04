
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190911111158_v1.29') THEN
    ALTER TABLE messages DROP COLUMN pharmacy_head_full_name;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190911111158_v1.29') THEN
    ALTER TABLE messages ADD pharmacy_head_last_name character varying(200) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190911111158_v1.29') THEN
    ALTER TABLE messages ADD pharmacy_head_middle_name character varying(200) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190911111158_v1.29') THEN
    ALTER TABLE messages ADD pharmacy_head_name character varying(100) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190911111158_v1.29') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190911111158_v1.29', '2.2.2-servicing-10034');
    END IF;
END $$;
