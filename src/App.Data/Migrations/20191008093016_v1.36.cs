using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v136 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_result_input_controls_iml_licenses_license_id",
                table: "result_input_controls");

            migrationBuilder.DropIndex(
                name: "ix_result_input_controls_license_id",
                table: "result_input_controls");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_result_input_controls_license_id",
                table: "result_input_controls",
                column: "license_id");

            migrationBuilder.AddForeignKey(
                name: "fk_result_input_controls_iml_licenses_license_id",
                table: "result_input_controls",
                column: "license_id",
                principalTable: "iml_licenses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
