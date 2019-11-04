using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace App.Data.Migrations
{
    public partial class v101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_assignees",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: true),
                    middle_name = table.Column<string>(maxLength: 200, nullable: true),
                    last_name = table.Column<string>(maxLength: 200, nullable: true),
                    ipn = table.Column<string>(maxLength: 50, nullable: true),
                    birthday = table.Column<DateTime>(nullable: true),
                    org_position_type = table.Column<string>(maxLength: 30, nullable: true),
                    education_institution = table.Column<string>(maxLength: 255, nullable: true),
                    year_of_graduation = table.Column<string>(maxLength: 10, nullable: true),
                    number_of_diploma = table.Column<string>(maxLength: 25, nullable: true),
                    date_of_graduation = table.Column<DateTime>(nullable: true),
                    speciality = table.Column<string>(maxLength: 200, nullable: true),
                    work_experience = table.Column<string>(maxLength: 5, nullable: true),
                    number_of_contract = table.Column<string>(maxLength: 20, nullable: true),
                    order_number = table.Column<string>(maxLength: 20, nullable: true),
                    date_of_contract = table.Column<DateTime>(nullable: true),
                    date_of_appointment = table.Column<DateTime>(nullable: true),
                    name_of_position = table.Column<string>(maxLength: 100, nullable: true),
                    contact_information = table.Column<string>(maxLength: 255, nullable: true),
                    comment = table.Column<string>(nullable: true),
                    license_assignee_id = table.Column<Guid>(nullable: true),
                    license_delete_check = table.Column<bool>(nullable: true),
                    is_from_license = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_assignees", x => x.id);
                    table.ForeignKey(
                        name: "fk_app_assignees_app_assignees_license_assignee_id",
                        column: x => x.license_assignee_id,
                        principalTable: "app_assignees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "app_license_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    message_number = table.Column<string>(nullable: false),
                    date_of_message = table.Column<DateTime>(nullable: false),
                    signed_job_position = table.Column<string>(nullable: true),
                    signed_full_name = table.Column<string>(nullable: true),
                    performer = table.Column<Guid>(nullable: false),
                    state = table.Column<string>(nullable: true),
                    attached_file = table.Column<string>(nullable: true),
                    old_lims_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_license_messages", x => x.id);
                    table.UniqueConstraint("ak_app_license_messages_app_id_message_number", x => new { x.app_id, x.message_number });
                });

            migrationBuilder.CreateTable(
                name: "app_pre_license_checks",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    scheduled_start_date = table.Column<DateTime>(nullable: false),
                    scheduled_end_date = table.Column<DateTime>(nullable: false),
                    check_created_id = table.Column<Guid>(nullable: false),
                    creation_date_of_check = table.Column<DateTime>(nullable: false),
                    end_date_of_check = table.Column<DateTime>(nullable: true),
                    result_of_check = table.Column<int>(nullable: true),
                    old_lims_id = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_pre_license_checks", x => x.id);
                    table.UniqueConstraint("ak_app_pre_license_checks_app_id", x => x.app_id);
                });

            migrationBuilder.CreateTable(
                name: "app_protocols",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    status_id = table.Column<int>(nullable: false),
                    status_name = table.Column<string>(nullable: true),
                    protocol_number = table.Column<string>(nullable: true),
                    protocol_date = table.Column<DateTime>(nullable: true),
                    order_number = table.Column<string>(nullable: true),
                    order_date = table.Column<DateTime>(nullable: true),
                    old_lims_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_protocols", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "atu_country",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    code = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_atu_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "atu_subject_address",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    street_id = table.Column<Guid>(nullable: false),
                    post_index = table.Column<string>(maxLength: 20, nullable: true),
                    building = table.Column<string>(maxLength: 300, nullable: true),
                    address_type = table.Column<string>(nullable: true),
                    subject_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_atu_subject_address", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_entries",
                columns: table => new
                {
                    audit_entry_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    created_by = table.Column<string>(maxLength: 255, nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    entity_set_name = table.Column<string>(maxLength: 255, nullable: true),
                    entity_type_name = table.Column<string>(maxLength: 255, nullable: true),
                    state = table.Column<int>(nullable: false),
                    state_name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_entries", x => x.audit_entry_id);
                });

            migrationBuilder.CreateTable(
                name: "cdn_position",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cdn_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cdn_speciality",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cdn_speciality", x => x.id);
                    table.ForeignKey(
                        name: "fk_cdn_speciality_cdn_speciality_parent_id",
                        column: x => x.parent_id,
                        principalTable: "cdn_speciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "edocuments",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    card_number = table.Column<string>(maxLength: 30, nullable: true),
                    date_from = table.Column<DateTime>(nullable: true),
                    date_to = table.Column<DateTime>(nullable: true),
                    version = table.Column<string>(maxLength: 30, nullable: true),
                    edocument_status = table.Column<string>(maxLength: 30, nullable: true),
                    comment = table.Column<string>(nullable: true),
                    edocument_type = table.Column<string>(nullable: true),
                    is_from_license = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_edocuments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entity_ex_property",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    entity_id = table.Column<Guid>(nullable: false),
                    ex_property_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: true),
                    value_ex = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entity_ex_property", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "enum_record",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    enum_type = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    ex_param1 = table.Column<string>(nullable: true),
                    ex_param2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_enum_record", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ex_property",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    prop_type_enum = table.Column<string>(nullable: true),
                    group = table.Column<string>(nullable: true),
                    kind_enum = table.Column<string>(nullable: true),
                    sort_order = table.Column<string>(nullable: true),
                    cotype_enum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ex_property", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_id = table.Column<Guid>(nullable: true),
                    app_sort = table.Column<string>(nullable: true),
                    rating = table.Column<int>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    org_id = table.Column<Guid>(nullable: true),
                    org_employee_id = table.Column<Guid>(nullable: true),
                    is_rated = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedbacks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file_store",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    file_type = table.Column<int>(nullable: false),
                    entity_name = table.Column<string>(nullable: true),
                    entity_id = table.Column<Guid>(nullable: false),
                    file_path = table.Column<string>(nullable: true),
                    file_name = table.Column<string>(nullable: true),
                    content_type = table.Column<string>(nullable: true),
                    orig_file_name = table.Column<string>(nullable: true),
                    file_size = table.Column<double>(nullable: false),
                    ock = table.Column<bool>(nullable: false),
                    document_type = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_store", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    parent_id = table.Column<Guid>(nullable: true),
                    date_of_create = table.Column<DateTime>(nullable: true),
                    notification_subject = table.Column<string>(maxLength: 300, nullable: true),
                    notification_type = table.Column<string>(maxLength: 10, nullable: true),
                    notification_text = table.Column<string>(maxLength: 10000, nullable: true),
                    recipient_json_list = table.Column<string>(maxLength: 10000, nullable: true),
                    is_send = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "org_organization",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    base_class = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    short_name = table.Column<string>(nullable: true),
                    code = table.Column<string>(maxLength: 20, nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    discriminator = table.Column<string>(nullable: false),
                    edrpou = table.Column<string>(maxLength: 30, nullable: true),
                    inn = table.Column<string>(maxLength: 30, nullable: true),
                    email = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_organization", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    middle_name = table.Column<string>(maxLength: 200, nullable: false),
                    last_name = table.Column<string>(maxLength: 200, nullable: false),
                    location = table.Column<string>(nullable: true),
                    birthday = table.Column<DateTime>(nullable: false),
                    gender_enum = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    user_id = table.Column<string>(nullable: true),
                    user_name = table.Column<string>(maxLength: 200, nullable: true),
                    photo = table.Column<string>(nullable: true),
                    description = table.Column<string>(maxLength: 1024, nullable: true),
                    ipn = table.Column<string>(maxLength: 50, nullable: true),
                    no_ipn = table.Column<bool>(nullable: false),
                    identity_document_type_enum = table.Column<string>(maxLength: 50, nullable: true),
                    doc_prefix = table.Column<string>(nullable: true),
                    personal_document_number = table.Column<string>(maxLength: 20, nullable: true),
                    personal_document_authority = table.Column<string>(maxLength: 100, nullable: true),
                    document_issue_date = table.Column<DateTime>(nullable: true),
                    expiration_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_person", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prl_contractors",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 100, nullable: true),
                    contractor_type = table.Column<string>(maxLength: 30, nullable: true),
                    edrpou = table.Column<string>(maxLength: 15, nullable: true),
                    address = table.Column<string>(maxLength: 200, nullable: true),
                    address_id = table.Column<Guid>(nullable: false),
                    license_contractor_id = table.Column<Guid>(nullable: true),
                    license_delete_check = table.Column<bool>(nullable: true),
                    is_from_license = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prl_contractors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_application_row_level_right",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    entity_name = table.Column<string>(nullable: true),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_application_row_level_right", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_profile",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_profile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_right",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    entity_name = table.Column<string>(maxLength: 64, nullable: true),
                    entity_access_level = table.Column<int>(nullable: false),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_right", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_role",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sys_application_setting",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    type_name = table.Column<string>(nullable: true),
                    is_enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_application_setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_decisions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    decision_type = table.Column<string>(nullable: true),
                    date_of_start = table.Column<DateTime>(nullable: false),
                    protocol_id = table.Column<Guid>(nullable: true),
                    decision_description = table.Column<string>(nullable: true),
                    paid_money = table.Column<decimal>(nullable: false),
                    notes = table.Column<string>(nullable: true),
                    is_closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_decisions", x => x.id);
                    table.UniqueConstraint("ak_app_decisions_app_id", x => x.app_id);
                    table.ForeignKey(
                        name: "fk_app_decisions_app_protocols_protocol_id",
                        column: x => x.protocol_id,
                        principalTable: "app_protocols",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "atu_region",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    parent_id = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    code = table.Column<string>(maxLength: 64, nullable: true),
                    country_id = table.Column<Guid>(nullable: false),
                    koatuu = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_atu_region", x => x.id);
                    table.ForeignKey(
                        name: "fk_atu_region_atu_country_country_id",
                        column: x => x.country_id,
                        principalTable: "atu_country",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_atu_region_atu_region_parent_id",
                        column: x => x.parent_id,
                        principalTable: "atu_region",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "org_unit",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    derived_class = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(maxLength: 20, nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    subject_address_id = table.Column<Guid>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_unit", x => x.id);
                    table.ForeignKey(
                        name: "fk_org_unit_atu_subject_address_subject_address_id",
                        column: x => x.subject_address_id,
                        principalTable: "atu_subject_address",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "audit_entry_properties",
                columns: table => new
                {
                    audit_entry_property_id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    audit_entry_id = table.Column<int>(nullable: false),
                    property_name = table.Column<string>(maxLength: 255, nullable: true),
                    relation_name = table.Column<string>(maxLength: 255, nullable: true),
                    new_value_formatted = table.Column<string>(nullable: true),
                    old_value_formatted = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_entry_properties", x => x.audit_entry_property_id);
                    table.ForeignKey(
                        name: "fk_audit_entry_properties_audit_entries_audit_entry_id",
                        column: x => x.audit_entry_id,
                        principalTable: "audit_entries",
                        principalColumn: "audit_entry_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "org_branches",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    base_class = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(maxLength: 20, nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    state = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    organization_id = table.Column<Guid>(nullable: false),
                    branch_state = table.Column<string>(maxLength: 20, nullable: true),
                    branch_activity = table.Column<string>(maxLength: 20, nullable: true),
                    phone_number = table.Column<string>(maxLength: 255, nullable: true),
                    fax_number = table.Column<string>(maxLength: 20, nullable: true),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    lims_license_branch_id = table.Column<int>(nullable: false),
                    address_id = table.Column<Guid>(nullable: false),
                    adress_eng = table.Column<string>(nullable: true),
                    license_delete_check = table.Column<bool>(nullable: true),
                    iml_is_availiable_storage_zone = table.Column<bool>(nullable: false),
                    iml_is_availiable_permit_issue_zone = table.Column<bool>(nullable: false),
                    iml_is_availiable_quality = table.Column<bool>(nullable: false),
                    iml_is_importing_finished = table.Column<bool>(nullable: false),
                    iml_is_importing_in_bulk = table.Column<bool>(nullable: false),
                    iml_another_activity = table.Column<string>(maxLength: 255, nullable: true),
                    branch_id = table.Column<Guid>(nullable: false),
                    trl_is_manufacture = table.Column<bool>(nullable: false),
                    trl_is_wholesale = table.Column<bool>(nullable: false),
                    trl_is_retail = table.Column<bool>(nullable: false),
                    prl_is_availiable_prod_sites = table.Column<bool>(nullable: false),
                    prl_is_availiable_quality_zone = table.Column<bool>(nullable: false),
                    prl_is_availiable_storage_zone = table.Column<bool>(nullable: false),
                    prl_is_availiable_pickup_zone = table.Column<bool>(nullable: false),
                    operation_list_form = table.Column<string>(nullable: true),
                    operation_list_form_changing = table.Column<string>(nullable: true),
                    is_from_license = table.Column<bool>(nullable: true),
                    old_lims_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_branches", x => x.id);
                    table.ForeignKey(
                        name: "fk_org_branches_org_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "org_organization_info",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    organization_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 10, nullable: true),
                    org_director = table.Column<string>(maxLength: 250, nullable: true),
                    legal_form_type = table.Column<string>(maxLength: 255, nullable: true),
                    ownership_type = table.Column<string>(maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(maxLength: 255, nullable: true),
                    fax_number = table.Column<string>(maxLength: 20, nullable: true),
                    address_id = table.Column<Guid>(nullable: false),
                    national_account = table.Column<string>(maxLength: 50, nullable: true),
                    international_account = table.Column<string>(maxLength: 50, nullable: true),
                    national_bank_requisites = table.Column<string>(maxLength: 255, nullable: true),
                    international_bank_requisites = table.Column<string>(maxLength: 255, nullable: true),
                    passport_serial = table.Column<string>(maxLength: 2, nullable: true),
                    passport_number = table.Column<string>(maxLength: 12, nullable: true),
                    passport_date = table.Column<DateTime>(nullable: true),
                    passport_issue_unit = table.Column<string>(maxLength: 200, nullable: true),
                    is_actual_info = table.Column<bool>(nullable: false),
                    is_pending_change_info = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_organization_info", x => x.id);
                    table.ForeignKey(
                        name: "fk_org_organization_info_org_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_row_level_right",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    profile_id = table.Column<Guid>(nullable: false),
                    entity_name = table.Column<string>(nullable: true),
                    access_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_row_level_right", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_row_level_right_sec_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "sec_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_field_right",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    right_id = table.Column<Guid>(nullable: false),
                    field_name = table.Column<string>(nullable: true),
                    access_level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_field_right", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_field_right_sec_right_right_id",
                        column: x => x.right_id,
                        principalTable: "sec_right",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_profile_right",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    profile_id = table.Column<Guid>(nullable: false),
                    right_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_profile_right", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_profile_right_sec_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "sec_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sec_profile_right_sec_right_right_id",
                        column: x => x.right_id,
                        principalTable: "sec_right",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_profile_role",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    profile_id = table.Column<Guid>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_profile_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_profile_role_sec_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "sec_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sec_profile_role_sec_role_role_id",
                        column: x => x.role_id,
                        principalTable: "sec_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_role_right",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false),
                    right_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_role_right", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_role_right_sec_right_right_id",
                        column: x => x.right_id,
                        principalTable: "sec_right",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sec_role_right_sec_role_role_id",
                        column: x => x.role_id,
                        principalTable: "sec_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sys_application_setting_value",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    application_setting_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_application_setting_value", x => x.id);
                    table.ForeignKey(
                        name: "fk_sys_application_setting_value_sys_application_setting_appli~",
                        column: x => x.application_setting_id,
                        principalTable: "sys_application_setting",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_decision_reasons",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_decision_id = table.Column<Guid>(nullable: false),
                    reason_type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_decision_reasons", x => x.id);
                    table.ForeignKey(
                        name: "fk_app_decision_reasons_app_decisions_app_decision_id",
                        column: x => x.app_decision_id,
                        principalTable: "app_decisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "atu_city",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: true),
                    code = table.Column<string>(nullable: true),
                    type_enum = table.Column<string>(nullable: true),
                    region_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_atu_city", x => x.id);
                    table.ForeignKey(
                        name: "fk_atu_city_atu_region_region_id",
                        column: x => x.region_id,
                        principalTable: "atu_region",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "org_unit_position",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    position_id = table.Column<Guid>(nullable: false),
                    position_type = table.Column<string>(nullable: true),
                    is_resource = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_unit_position", x => x.id);
                    table.ForeignKey(
                        name: "fk_org_unit_position_org_unit_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_org_unit_position_cdn_position_position_id",
                        column: x => x.position_id,
                        principalTable: "cdn_position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_assignee_branches",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    assignee_id = table.Column<Guid>(nullable: false),
                    branch_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_assignee_branches", x => x.id);
                    table.ForeignKey(
                        name: "fk_app_assignee_branches_app_assignees_assignee_id",
                        column: x => x.assignee_id,
                        principalTable: "app_assignees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_assignee_branches_org_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "branch_edocuments",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    branch_id = table.Column<Guid>(nullable: false),
                    edocument_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_branch_edocuments", x => x.id);
                    table.ForeignKey(
                        name: "fk_branch_edocuments_org_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_branch_edocuments_edocuments_edocument_id",
                        column: x => x.edocument_id,
                        principalTable: "edocuments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prl_branch_contractors",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    branch_id = table.Column<Guid>(nullable: false),
                    contractor_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prl_branch_contractors", x => x.id);
                    table.ForeignKey(
                        name: "fk_prl_branch_contractors_org_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prl_branch_contractors_prl_contractors_contractor_id",
                        column: x => x.contractor_id,
                        principalTable: "prl_contractors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_row_level_security_object",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    row_level_right_id = table.Column<Guid>(nullable: false),
                    entity_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_row_level_security_object", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_row_level_security_object_sec_row_level_right_row_level~",
                        column: x => x.row_level_right_id,
                        principalTable: "sec_row_level_right",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "atu_city_districts",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: true),
                    city_id = table.Column<Guid>(nullable: false),
                    code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_atu_city_districts", x => x.id);
                    table.ForeignKey(
                        name: "fk_atu_city_districts_atu_city_city_id",
                        column: x => x.city_id,
                        principalTable: "atu_city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "atu_street",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    city_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_atu_street", x => x.id);
                    table.ForeignKey(
                        name: "fk_atu_street_atu_city_city_id",
                        column: x => x.city_id,
                        principalTable: "atu_city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "org_employee",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    person_id = table.Column<Guid>(nullable: false),
                    organization_id = table.Column<Guid>(nullable: true),
                    discriminator = table.Column<string>(nullable: false),
                    org_unit_position_id = table.Column<Guid>(nullable: true),
                    org_unit_specialization_id = table.Column<Guid>(nullable: true),
                    atu_region_id = table.Column<Guid>(nullable: true),
                    user_email = table.Column<string>(nullable: true),
                    position = table.Column<string>(nullable: true),
                    receive_on_change_all_application = table.Column<bool>(nullable: true),
                    receive_on_change_all_message = table.Column<bool>(nullable: true),
                    receive_on_change_own_application = table.Column<bool>(nullable: true),
                    receive_on_change_own_message = table.Column<bool>(nullable: true),
                    personal_cabinet_status = table.Column<bool>(nullable: true),
                    receive_on_change_org_info = table.Column<bool>(nullable: true),
                    receive_on_overdue_payment = table.Column<bool>(nullable: true),
                    old_lims_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_employee", x => x.id);
                    table.ForeignKey(
                        name: "fk_org_employee_org_unit_position_org_unit_position_id",
                        column: x => x.org_unit_position_id,
                        principalTable: "org_unit_position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_org_employee_org_organization_organization_id",
                        column: x => x.organization_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_org_employee_person_person_id",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cdn_employee_speciality",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    employee_id = table.Column<Guid>(nullable: false),
                    speciality_id = table.Column<Guid>(nullable: false),
                    is_main_speciality = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cdn_employee_speciality", x => x.id);
                    table.ForeignKey(
                        name: "fk_cdn_employee_speciality_org_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cdn_employee_speciality_cdn_speciality_speciality_id",
                        column: x => x.speciality_id,
                        principalTable: "cdn_speciality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lims_docs",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    derived_class = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    applicant_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    fax_number = table.Column<string>(maxLength: 20, nullable: true),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    phone = table.Column<string>(maxLength: 24, nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(maxLength: 2000, nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lims_docs", x => x.id);
                    table.ForeignKey(
                        name: "fk_lims_docs_org_organization_applicant_id",
                        column: x => x.applicant_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lims_docs_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_lims_docs_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_lims_docs_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "org_unit_position_employee",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    org_unit_position_id = table.Column<Guid>(nullable: false),
                    employee_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_org_unit_position_employee", x => x.id);
                    table.ForeignKey(
                        name: "fk_org_unit_position_employee_org_employee_employee_id",
                        column: x => x.employee_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_org_unit_position_employee_org_unit_position_org_unit_posit~",
                        column: x => x.org_unit_position_id,
                        principalTable: "org_unit_position",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_user_default_value",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    entity_name = table.Column<string>(nullable: true),
                    value_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_user_default_value", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_user_default_value_org_employee_user_id",
                        column: x => x.user_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sec_user_profile",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    profile_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_user_profile", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_user_profile_sec_profile_profile_id",
                        column: x => x.profile_id,
                        principalTable: "sec_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sec_user_profile_org_employee_user_id",
                        column: x => x.user_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "application_branches",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    branch_id = table.Column<Guid>(nullable: true),
                    lims_document_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_branches", x => x.id);
                    table.ForeignKey(
                        name: "fk_application_branches_org_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_application_branches_lims_docs_lims_document_id",
                        column: x => x.lims_document_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "iml_applications",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_type = table.Column<string>(maxLength: 30, nullable: true),
                    app_sort = table.Column<string>(maxLength: 40, nullable: true),
                    app_state = table.Column<string>(maxLength: 30, nullable: true),
                    back_office_app_state = table.Column<string>(maxLength: 30, nullable: true),
                    app_decision_id = table.Column<Guid>(nullable: true),
                    app_pre_license_check_id = table.Column<Guid>(nullable: true),
                    app_license_message_id = table.Column<Guid>(nullable: true),
                    app_decision_notes = table.Column<string>(nullable: true),
                    is_check_mpd = table.Column<bool>(nullable: false),
                    is_paper_license = table.Column<bool>(nullable: false),
                    is_courier_delivery = table.Column<bool>(nullable: false),
                    is_post_delivery = table.Column<bool>(nullable: false),
                    is_agree_license_terms = table.Column<bool>(nullable: false),
                    is_agree_processing_data = table.Column<bool>(nullable: false),
                    is_protection_from_aggressors = table.Column<bool>(nullable: false),
                    is_courier_results = table.Column<bool>(nullable: false),
                    is_post_results = table.Column<bool>(nullable: false),
                    is_electric_form_results = table.Column<bool>(nullable: false),
                    duns = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    is_created_on_portal = table.Column<bool>(nullable: false),
                    expertise_result = table.Column<string>(nullable: true),
                    expertise_date = table.Column<DateTime>(nullable: true),
                    performer_of_expertise = table.Column<Guid>(nullable: true),
                    error_processing_license = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iml_applications", x => x.id);
                    table.ForeignKey(
                        name: "fk_iml_applications_app_decisions_app_decision_id",
                        column: x => x.app_decision_id,
                        principalTable: "app_decisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_iml_applications_app_license_messages_app_license_message_id",
                        column: x => x.app_license_message_id,
                        principalTable: "app_license_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_iml_applications_app_pre_license_checks_app_pre_license_che~",
                        column: x => x.app_pre_license_check_id,
                        principalTable: "app_pre_license_checks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_iml_applications_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_iml_applications_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_iml_applications_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "iml_licenses",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    lic_type = table.Column<string>(maxLength: 30, nullable: true),
                    lic_sort = table.Column<string>(maxLength: 40, nullable: true),
                    lic_state = table.Column<string>(maxLength: 30, nullable: true),
                    license_number = table.Column<string>(nullable: true),
                    license_date = table.Column<string>(nullable: true),
                    order_number = table.Column<string>(nullable: true),
                    order_date = table.Column<string>(nullable: true),
                    is_relevant = table.Column<bool>(nullable: false),
                    end_reason_text = table.Column<string>(nullable: true),
                    end_order_number = table.Column<string>(nullable: true),
                    end_order_date = table.Column<DateTime>(nullable: true),
                    end_order_text = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iml_licenses", x => x.id);
                    table.ForeignKey(
                        name: "fk_iml_licenses_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_iml_licenses_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_iml_licenses_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true),
                    message_parent_id = table.Column<Guid>(nullable: false),
                    is_created_on_portal = table.Column<bool>(nullable: false),
                    is_prl_license = table.Column<bool>(nullable: false),
                    is_trl_license = table.Column<bool>(nullable: false),
                    is_iml_license = table.Column<bool>(nullable: false),
                    message_type = table.Column<string>(nullable: true),
                    message_hierarchy_type = table.Column<string>(nullable: true),
                    message_number = table.Column<string>(nullable: true),
                    message_text = table.Column<string>(nullable: true),
                    message_state = table.Column<string>(nullable: true),
                    mpd_selected_id = table.Column<Guid>(nullable: false),
                    new_sgd_name = table.Column<string>(nullable: true),
                    sgd_shief_full_name = table.Column<string>(nullable: true),
                    sgd_new_full_name = table.Column<string>(nullable: true),
                    new_location_id = table.Column<Guid>(nullable: false),
                    restoration_date = table.Column<DateTime>(nullable: true),
                    restoration_reason = table.Column<string>(nullable: true),
                    suspension_start_date = table.Column<DateTime>(nullable: true),
                    suspension_reason = table.Column<string>(nullable: true),
                    closing_date = table.Column<DateTime>(nullable: true),
                    closing_reason = table.Column<string>(nullable: true),
                    address_business_activity_id = table.Column<Guid>(nullable: false),
                    pharmacy_head_full_name = table.Column<string>(nullable: true),
                    new_pharmacy_area = table.Column<long>(nullable: true),
                    new_pharmacy_name = table.Column<string>(nullable: true),
                    new_legal_entity = table.Column<string>(nullable: true),
                    lease_agreement_start_date = table.Column<DateTime>(nullable: true),
                    lease_agreement_end_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_messages_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_messages_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_messages_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prl_applications",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_type = table.Column<string>(maxLength: 30, nullable: true),
                    app_sort = table.Column<string>(maxLength: 40, nullable: true),
                    app_state = table.Column<string>(maxLength: 30, nullable: true),
                    back_office_app_state = table.Column<string>(maxLength: 30, nullable: true),
                    app_decision_id = table.Column<Guid>(nullable: true),
                    app_pre_license_check_id = table.Column<Guid>(nullable: true),
                    app_license_message_id = table.Column<Guid>(nullable: true),
                    app_decision_notes = table.Column<string>(nullable: true),
                    is_check_mpd = table.Column<bool>(nullable: false),
                    is_paper_license = table.Column<bool>(nullable: false),
                    is_courier_delivery = table.Column<bool>(nullable: false),
                    is_post_delivery = table.Column<bool>(nullable: false),
                    is_agree_license_terms = table.Column<bool>(nullable: false),
                    is_agree_processing_data = table.Column<bool>(nullable: false),
                    is_protection_from_aggressors = table.Column<bool>(nullable: false),
                    is_courier_results = table.Column<bool>(nullable: false),
                    is_post_results = table.Column<bool>(nullable: false),
                    is_electric_form_results = table.Column<bool>(nullable: false),
                    duns = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    is_created_on_portal = table.Column<bool>(nullable: false),
                    expertise_result = table.Column<string>(nullable: true),
                    expertise_date = table.Column<DateTime>(nullable: true),
                    performer_of_expertise = table.Column<Guid>(nullable: true),
                    error_processing_license = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prl_applications", x => x.id);
                    table.ForeignKey(
                        name: "fk_prl_applications_app_decisions_app_decision_id",
                        column: x => x.app_decision_id,
                        principalTable: "app_decisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prl_applications_app_license_messages_app_license_message_id",
                        column: x => x.app_license_message_id,
                        principalTable: "app_license_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prl_applications_app_pre_license_checks_app_pre_license_che~",
                        column: x => x.app_pre_license_check_id,
                        principalTable: "app_pre_license_checks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prl_applications_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prl_applications_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prl_applications_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "prl_licenses",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    lic_type = table.Column<string>(maxLength: 30, nullable: true),
                    lic_sort = table.Column<string>(maxLength: 40, nullable: true),
                    lic_state = table.Column<string>(maxLength: 30, nullable: true),
                    license_number = table.Column<string>(nullable: true),
                    license_date = table.Column<string>(nullable: true),
                    order_number = table.Column<string>(nullable: true),
                    order_date = table.Column<string>(nullable: true),
                    is_relevant = table.Column<bool>(nullable: false),
                    end_reason_text = table.Column<string>(nullable: true),
                    end_order_number = table.Column<string>(nullable: true),
                    end_order_date = table.Column<DateTime>(nullable: true),
                    end_order_text = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prl_licenses", x => x.id);
                    table.ForeignKey(
                        name: "fk_prl_licenses_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prl_licenses_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_prl_licenses_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trl_applications",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    app_type = table.Column<string>(maxLength: 30, nullable: true),
                    app_sort = table.Column<string>(maxLength: 40, nullable: true),
                    app_state = table.Column<string>(maxLength: 30, nullable: true),
                    back_office_app_state = table.Column<string>(maxLength: 30, nullable: true),
                    app_decision_id = table.Column<Guid>(nullable: true),
                    app_pre_license_check_id = table.Column<Guid>(nullable: true),
                    app_license_message_id = table.Column<Guid>(nullable: true),
                    app_decision_notes = table.Column<string>(nullable: true),
                    is_check_mpd = table.Column<bool>(nullable: false),
                    is_paper_license = table.Column<bool>(nullable: false),
                    is_courier_delivery = table.Column<bool>(nullable: false),
                    is_post_delivery = table.Column<bool>(nullable: false),
                    is_agree_license_terms = table.Column<bool>(nullable: false),
                    is_agree_processing_data = table.Column<bool>(nullable: false),
                    is_protection_from_aggressors = table.Column<bool>(nullable: false),
                    is_courier_results = table.Column<bool>(nullable: false),
                    is_post_results = table.Column<bool>(nullable: false),
                    is_electric_form_results = table.Column<bool>(nullable: false),
                    duns = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true),
                    is_created_on_portal = table.Column<bool>(nullable: false),
                    expertise_result = table.Column<string>(nullable: true),
                    expertise_date = table.Column<DateTime>(nullable: true),
                    performer_of_expertise = table.Column<Guid>(nullable: true),
                    error_processing_license = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trl_applications", x => x.id);
                    table.ForeignKey(
                        name: "fk_trl_applications_app_decisions_app_decision_id",
                        column: x => x.app_decision_id,
                        principalTable: "app_decisions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trl_applications_app_license_messages_app_license_message_id",
                        column: x => x.app_license_message_id,
                        principalTable: "app_license_messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trl_applications_app_pre_license_checks_app_pre_license_che~",
                        column: x => x.app_pre_license_check_id,
                        principalTable: "app_pre_license_checks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trl_applications_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trl_applications_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trl_applications_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trl_licenses",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    lic_type = table.Column<string>(maxLength: 30, nullable: true),
                    lic_sort = table.Column<string>(maxLength: 40, nullable: true),
                    lic_state = table.Column<string>(maxLength: 30, nullable: true),
                    license_number = table.Column<string>(nullable: true),
                    license_date = table.Column<string>(nullable: true),
                    order_number = table.Column<string>(nullable: true),
                    order_date = table.Column<string>(nullable: true),
                    is_relevant = table.Column<bool>(nullable: false),
                    end_reason_text = table.Column<string>(nullable: true),
                    end_order_number = table.Column<string>(nullable: true),
                    end_order_date = table.Column<DateTime>(nullable: true),
                    end_order_text = table.Column<string>(nullable: true),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trl_licenses", x => x.id);
                    table.ForeignKey(
                        name: "fk_trl_licenses_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trl_licenses_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trl_licenses_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_assignee_branches_assignee_id",
                table: "app_assignee_branches",
                column: "assignee_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_assignee_branches_branch_id",
                table: "app_assignee_branches",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_assignees_license_assignee_id",
                table: "app_assignees",
                column: "license_assignee_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_decision_reasons_app_decision_id",
                table: "app_decision_reasons",
                column: "app_decision_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_decisions_protocol_id",
                table: "app_decisions",
                column: "protocol_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_branches_branch_id",
                table: "application_branches",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_application_branches_lims_document_id",
                table: "application_branches",
                column: "lims_document_id");

            migrationBuilder.CreateIndex(
                name: "ix_atu_city_region_id",
                table: "atu_city",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_atu_city_name_code",
                table: "atu_city",
                columns: new[] { "name", "code" });

            migrationBuilder.CreateIndex(
                name: "ix_atu_city_districts_city_id",
                table: "atu_city_districts",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_atu_region_country_id",
                table: "atu_region",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_atu_region_parent_id",
                table: "atu_region",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_atu_street_city_id",
                table: "atu_street",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_entry_properties_audit_entry_id",
                table: "audit_entry_properties",
                column: "audit_entry_id");

            migrationBuilder.CreateIndex(
                name: "ix_branch_edocuments_branch_id",
                table: "branch_edocuments",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_branch_edocuments_edocument_id",
                table: "branch_edocuments",
                column: "edocument_id");

            migrationBuilder.CreateIndex(
                name: "ix_cdn_employee_speciality_employee_id",
                table: "cdn_employee_speciality",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_cdn_employee_speciality_speciality_id",
                table: "cdn_employee_speciality",
                column: "speciality_id");

            migrationBuilder.CreateIndex(
                name: "ix_cdn_speciality_parent_id",
                table: "cdn_speciality",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_enum_record_enum_type_code",
                table: "enum_record",
                columns: new[] { "enum_type", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_app_decision_id",
                table: "iml_applications",
                column: "app_decision_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_app_license_message_id",
                table: "iml_applications",
                column: "app_license_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_app_pre_license_check_id",
                table: "iml_applications",
                column: "app_pre_license_check_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_org_unit_id",
                table: "iml_applications",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_parent_id",
                table: "iml_applications",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_performer_id",
                table: "iml_applications",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_licenses_org_unit_id",
                table: "iml_licenses",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_licenses_parent_id",
                table: "iml_licenses",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_licenses_performer_id",
                table: "iml_licenses",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_lims_docs_applicant_id",
                table: "lims_docs",
                column: "applicant_id");

            migrationBuilder.CreateIndex(
                name: "ix_lims_docs_org_unit_id",
                table: "lims_docs",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_lims_docs_parent_id",
                table: "lims_docs",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_lims_docs_performer_id",
                table: "lims_docs",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_org_unit_id",
                table: "messages",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_parent_id",
                table: "messages",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_performer_id",
                table: "messages",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_reg_number",
                table: "messages",
                column: "reg_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_org_branches_organization_id",
                table: "org_branches",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_employee_org_unit_position_id",
                table: "org_employee",
                column: "org_unit_position_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_employee_organization_id",
                table: "org_employee",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_employee_person_id",
                table: "org_employee",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_organization_info_organization_id",
                table: "org_organization_info",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_unit_subject_address_id",
                table: "org_unit",
                column: "subject_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_unit_position_org_unit_id",
                table: "org_unit_position",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_unit_position_position_id",
                table: "org_unit_position",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_unit_position_employee_employee_id",
                table: "org_unit_position_employee",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_org_unit_position_employee_org_unit_position_id",
                table: "org_unit_position_employee",
                column: "org_unit_position_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_app_decision_id",
                table: "prl_applications",
                column: "app_decision_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_app_license_message_id",
                table: "prl_applications",
                column: "app_license_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_app_pre_license_check_id",
                table: "prl_applications",
                column: "app_pre_license_check_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_org_unit_id",
                table: "prl_applications",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_parent_id",
                table: "prl_applications",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_performer_id",
                table: "prl_applications",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_branch_contractors_branch_id",
                table: "prl_branch_contractors",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_branch_contractors_contractor_id",
                table: "prl_branch_contractors",
                column: "contractor_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_licenses_org_unit_id",
                table: "prl_licenses",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_licenses_parent_id",
                table: "prl_licenses",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_licenses_performer_id",
                table: "prl_licenses",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_field_right_right_id",
                table: "sec_field_right",
                column: "right_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_profile_right_profile_id",
                table: "sec_profile_right",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_profile_right_right_id",
                table: "sec_profile_right",
                column: "right_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_profile_role_profile_id",
                table: "sec_profile_role",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_profile_role_role_id",
                table: "sec_profile_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_role_right_right_id",
                table: "sec_role_right",
                column: "right_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_role_right_role_id",
                table: "sec_role_right",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_row_level_right_profile_id",
                table: "sec_row_level_right",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_row_level_security_object_row_level_right_id",
                table: "sec_row_level_security_object",
                column: "row_level_right_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_user_default_value_user_id",
                table: "sec_user_default_value",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_user_profile_profile_id",
                table: "sec_user_profile",
                column: "profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_user_profile_user_id",
                table: "sec_user_profile",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_sys_application_setting_value_application_setting_id",
                table: "sys_application_setting_value",
                column: "application_setting_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_app_decision_id",
                table: "trl_applications",
                column: "app_decision_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_app_license_message_id",
                table: "trl_applications",
                column: "app_license_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_app_pre_license_check_id",
                table: "trl_applications",
                column: "app_pre_license_check_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_org_unit_id",
                table: "trl_applications",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_parent_id",
                table: "trl_applications",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_performer_id",
                table: "trl_applications",
                column: "performer_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_licenses_org_unit_id",
                table: "trl_licenses",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_licenses_parent_id",
                table: "trl_licenses",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_trl_licenses_performer_id",
                table: "trl_licenses",
                column: "performer_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_assignee_branches");

            migrationBuilder.DropTable(
                name: "app_decision_reasons");

            migrationBuilder.DropTable(
                name: "application_branches");

            migrationBuilder.DropTable(
                name: "atu_city_districts");

            migrationBuilder.DropTable(
                name: "atu_street");

            migrationBuilder.DropTable(
                name: "audit_entry_properties");

            migrationBuilder.DropTable(
                name: "branch_edocuments");

            migrationBuilder.DropTable(
                name: "cdn_employee_speciality");

            migrationBuilder.DropTable(
                name: "entity_ex_property");

            migrationBuilder.DropTable(
                name: "enum_record");

            migrationBuilder.DropTable(
                name: "ex_property");

            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "file_store");

            migrationBuilder.DropTable(
                name: "iml_applications");

            migrationBuilder.DropTable(
                name: "iml_licenses");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "org_organization_info");

            migrationBuilder.DropTable(
                name: "org_unit_position_employee");

            migrationBuilder.DropTable(
                name: "prl_applications");

            migrationBuilder.DropTable(
                name: "prl_branch_contractors");

            migrationBuilder.DropTable(
                name: "prl_licenses");

            migrationBuilder.DropTable(
                name: "sec_application_row_level_right");

            migrationBuilder.DropTable(
                name: "sec_field_right");

            migrationBuilder.DropTable(
                name: "sec_profile_right");

            migrationBuilder.DropTable(
                name: "sec_profile_role");

            migrationBuilder.DropTable(
                name: "sec_role_right");

            migrationBuilder.DropTable(
                name: "sec_row_level_security_object");

            migrationBuilder.DropTable(
                name: "sec_user_default_value");

            migrationBuilder.DropTable(
                name: "sec_user_profile");

            migrationBuilder.DropTable(
                name: "sys_application_setting_value");

            migrationBuilder.DropTable(
                name: "trl_applications");

            migrationBuilder.DropTable(
                name: "trl_licenses");

            migrationBuilder.DropTable(
                name: "app_assignees");

            migrationBuilder.DropTable(
                name: "atu_city");

            migrationBuilder.DropTable(
                name: "audit_entries");

            migrationBuilder.DropTable(
                name: "edocuments");

            migrationBuilder.DropTable(
                name: "cdn_speciality");

            migrationBuilder.DropTable(
                name: "org_branches");

            migrationBuilder.DropTable(
                name: "prl_contractors");

            migrationBuilder.DropTable(
                name: "sec_right");

            migrationBuilder.DropTable(
                name: "sec_role");

            migrationBuilder.DropTable(
                name: "sec_row_level_right");

            migrationBuilder.DropTable(
                name: "sys_application_setting");

            migrationBuilder.DropTable(
                name: "app_decisions");

            migrationBuilder.DropTable(
                name: "app_license_messages");

            migrationBuilder.DropTable(
                name: "app_pre_license_checks");

            migrationBuilder.DropTable(
                name: "lims_docs");

            migrationBuilder.DropTable(
                name: "atu_region");

            migrationBuilder.DropTable(
                name: "sec_profile");

            migrationBuilder.DropTable(
                name: "app_protocols");

            migrationBuilder.DropTable(
                name: "org_employee");

            migrationBuilder.DropTable(
                name: "atu_country");

            migrationBuilder.DropTable(
                name: "org_unit_position");

            migrationBuilder.DropTable(
                name: "org_organization");

            migrationBuilder.DropTable(
                name: "person");

            migrationBuilder.DropTable(
                name: "org_unit");

            migrationBuilder.DropTable(
                name: "cdn_position");

            migrationBuilder.DropTable(
                name: "atu_subject_address");
        }
    }
}
