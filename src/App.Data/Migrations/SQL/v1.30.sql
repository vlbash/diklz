
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190912135246_v1.30') THEN
    ALTER TABLE messages RENAME COLUMN new_sgd_name TO sgd_shief_old_full_name;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190912135246_v1.30') THEN
    ALTER TABLE messages ADD old_location_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190912135246_v1.30') THEN
    ALTER TABLE messages ADD sgd_old_full_name text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190912135246_v1.30') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190912135246_v1.30', '2.2.2-servicing-10034');
    END IF;
END $$;
