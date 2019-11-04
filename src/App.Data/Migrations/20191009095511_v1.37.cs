using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v137 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "area_of_common_premises",
                table: "org_branches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "total_area",
                table: "org_branches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "area_of_common_premises",
                table: "org_branches");

            migrationBuilder.DropColumn(
                name: "total_area",
                table: "org_branches");
        }
    }
}
