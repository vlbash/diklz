using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v139 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "return_check",
                table: "trl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "return_comment",
                table: "trl_applications",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "return_check",
                table: "prl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "return_comment",
                table: "prl_applications",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "return_check",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "return_comment",
                table: "iml_applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "return_check",
                table: "trl_applications");

            migrationBuilder.DropColumn(
                name: "return_comment",
                table: "trl_applications");

            migrationBuilder.DropColumn(
                name: "return_check",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "return_comment",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "return_check",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "return_comment",
                table: "iml_applications");
        }
    }
}
