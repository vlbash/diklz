
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190919130206_v1.31') THEN
    ALTER TABLE trl_applications ADD expertise_comment text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190919130206_v1.31') THEN
    ALTER TABLE prl_applications ADD expertise_comment text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190919130206_v1.31') THEN
    ALTER TABLE iml_applications ADD expertise_comment text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190919130206_v1.31') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190919130206_v1.31', '2.2.2-servicing-10034');
    END IF;
END $$;
