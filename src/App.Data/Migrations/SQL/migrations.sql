
    ALTER TABLE feedbacks ALTER COLUMN id TYPE uuid;
    ALTER TABLE feedbacks ALTER COLUMN id SET NOT NULL;
    DROP SEQUENCE feedbacks_id_seq CASCADE;
    ALTER TABLE feedbacks ALTER COLUMN id DROP DEFAULT;
    DROP TABLE feedbacks;
    CREATE TABLE feedbacks (
        id uuid NOT NULL,
        record_state integer NOT NULL,
        caption character varying(128) NULL,
        modified_by uuid NOT NULL,
        modified_on timestamp without time zone NULL,
        created_by uuid NOT NULL,
        created_on timestamp without time zone NOT NULL,
        app_id uuid NULL,
        app_sort text NULL,
        rating integer NOT NULL,
        comment text NULL,
        org_id uuid NULL,
        org_employee_id uuid NULL,
        is_rated boolean NOT NULL,
        CONSTRAINT pk_feedbacks PRIMARY KEY (id)
    );
    ALTER TABLE org_employee ADD receive_on_change_org_info boolean NULL;
    ALTER TABLE org_employee ADD receive_on_overdue_payment boolean NULL;
    ALTER TABLE edocuments ALTER COLUMN edocument_status TYPE character varying(30);
    ALTER TABLE edocuments ALTER COLUMN edocument_status DROP NOT NULL;
    ALTER TABLE edocuments ALTER COLUMN edocument_status DROP DEFAULT;
    ALTER TABLE notifications ADD notification_type character varying(10) NULL;
    ALTER TABLE org_organization_info ADD is_pending_change_info boolean NOT NULL DEFAULT FALSE;
