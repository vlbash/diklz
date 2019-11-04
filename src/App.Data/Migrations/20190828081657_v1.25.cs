using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "aseptic_conditions",
                table: "org_branches",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "special_conditions",
                table: "org_branches",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aseptic_conditions",
                table: "org_branches");

            migrationBuilder.DropColumn(
                name: "special_conditions",
                table: "org_branches");
        }
    }
}
