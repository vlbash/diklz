using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "prl_in_pharmacies",
                table: "trl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "retail_of_medicines",
                table: "trl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "wholesale_of_medicines",
                table: "trl_applications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prl_in_pharmacies",
                table: "trl_applications");

            migrationBuilder.DropColumn(
                name: "retail_of_medicines",
                table: "trl_applications");

            migrationBuilder.DropColumn(
                name: "wholesale_of_medicines",
                table: "trl_applications");
        }
    }
}
