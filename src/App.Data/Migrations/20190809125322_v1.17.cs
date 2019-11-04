using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v117 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_from_license",
                table: "iml_medicines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_iml_medicines_application_id",
                table: "iml_medicines",
                column: "application_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_iml_medicines_application_id",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "is_from_license",
                table: "iml_medicines");
        }
    }
}
