using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v141 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conclusion_medicines",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    application_id = table.Column<Guid>(nullable: false),
                    conclusion_application_id = table.Column<Guid>(nullable: true),
                    medicine_name = table.Column<string>(nullable: true),
                    form_name = table.Column<string>(nullable: true),
                    dose_in_unit = table.Column<string>(nullable: true),
                    number_of_units = table.Column<string>(nullable: true),
                    medicine_name_eng = table.Column<string>(nullable: true),
                    register_number = table.Column<string>(nullable: true),
                    atc_code = table.Column<string>(nullable: true),
                    producer_name = table.Column<string>(nullable: true),
                    producer_country = table.Column<string>(nullable: true),
                    supplier_name = table.Column<string>(nullable: true),
                    supplier_country = table.Column<string>(nullable: true),
                    supplier_address = table.Column<string>(nullable: true),
                    is_from_license = table.Column<bool>(nullable: false),
                    lims_rp_id = table.Column<Guid>(nullable: false),
                    notes = table.Column<string>(nullable: true),
                    old_drug_id = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conclusion_medicines", x => x.id);
                    table.ForeignKey(
                        name: "fk_conclusion_medicines_app_conclusions_conclusion_application~",
                        column: x => x.conclusion_application_id,
                        principalTable: "app_conclusions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_conclusion_medicines_lims_rp_lims_rp_id",
                        column: x => x.lims_rp_id,
                        principalTable: "lims_rp",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_conclusion_medicines_conclusion_application_id",
                table: "conclusion_medicines",
                column: "conclusion_application_id");

            migrationBuilder.CreateIndex(
                name: "ix_conclusion_medicines_lims_rp_id",
                table: "conclusion_medicines",
                column: "lims_rp_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conclusion_medicines");
        }
    }
}
