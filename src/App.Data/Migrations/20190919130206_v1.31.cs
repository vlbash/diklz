using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v131 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "expertise_comment",
                table: "trl_applications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expertise_comment",
                table: "prl_applications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "expertise_comment",
                table: "iml_applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expertise_comment",
                table: "trl_applications");

            migrationBuilder.DropColumn(
                name: "expertise_comment",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "expertise_comment",
                table: "iml_applications");
        }
    }
}
