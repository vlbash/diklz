
DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190717153637_v1.08') THEN
    CREATE TABLE lims_rp (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        doc_id integer NOT NULL,
        reg_num text NULL,
        reg_proc_code text NULL,
        state_id integer NULL,
        reg_date timestamp without time zone NULL,
        end_date timestamp without time zone NULL,
        ord_reg_num text NULL,
        ord_reg_date timestamp without time zone NULL,
        drug_name_ukr text NULL,
        drug_name_eng text NULL,
        form_type_id integer NULL,
        formtype_desc text NULL,
        form_name text NULL,
        farm_group text NULL,
        side_name text NULL,
        country_id integer NULL,
        country_name text NULL,
        producer_name text NULL,
        prod_country_name text NULL,
        is_resident boolean NOT NULL,
        reg_procedure text NULL,
        regproc_id integer NULL,
        regproc_name text NULL,
        regproc_code text NULL,
        drugtype_id integer NULL,
        drugtype_name text NULL,
        rp_order_id integer NULL,
        off_order_num text NULL,
        off_order_date timestamp without time zone NULL,
        off_reason text NULL,
        drug_class_id integer NULL,
        drug_class_name text NULL,
        atc_code text NULL,
        active_substances text NULL,
        sale_terms text NULL,
        publicity_info text NULL,
        notes text NULL,
        CONSTRAINT pk_lims_rp PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20190717153637_v1.08') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20190717153637_v1.08', '2.2.2-servicing-10034');
    END IF;
END $$;
