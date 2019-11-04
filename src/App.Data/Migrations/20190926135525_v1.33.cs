using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v133 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "result_input_controls",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    state = table.Column<string>(nullable: true),
                    teritorial_service = table.Column<string>(nullable: true),
                    license_id = table.Column<Guid>(nullable: false),
                    lims_rpid = table.Column<Guid>(nullable: false),
                    register_number = table.Column<string>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    drug_name = table.Column<string>(nullable: true),
                    drug_form = table.Column<string>(nullable: true),
                    producer_name = table.Column<string>(nullable: true),
                    producer_country = table.Column<string>(nullable: true),
                    medicine_series = table.Column<string>(nullable: true),
                    medicine_expiration_date = table.Column<DateTime>(nullable: true),
                    size_of_series = table.Column<string>(nullable: true),
                    unit_of_measurement = table.Column<string>(nullable: true),
                    amount_of_imported_medicine = table.Column<string>(nullable: true),
                    win_number = table.Column<string>(nullable: true),
                    date_win = table.Column<DateTime>(nullable: true),
                    input_control_result = table.Column<string>(nullable: true),
                    name_of_mismatch = table.Column<string>(nullable: true),
                    comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_result_input_controls", x => x.id);
                    table.ForeignKey(
                        name: "fk_result_input_controls_iml_licenses_license_id",
                        column: x => x.license_id,
                        principalTable: "iml_licenses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_result_input_controls_lims_rp_lims_rpid",
                        column: x => x.lims_rpid,
                        principalTable: "lims_rp",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_result_input_controls_license_id",
                table: "result_input_controls",
                column: "license_id");

            migrationBuilder.CreateIndex(
                name: "ix_result_input_controls_lims_rpid",
                table: "result_input_controls",
                column: "lims_rpid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "result_input_controls");
        }
    }
}
