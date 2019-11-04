using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v124 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iml_another_activity",
                table: "org_branches");

            migrationBuilder.DropColumn(
                name: "iml_is_importing_finished",
                table: "org_branches");

            migrationBuilder.DropColumn(
                name: "iml_is_importing_in_bulk",
                table: "org_branches");

            migrationBuilder.AddColumn<string>(
                name: "iml_another_activity",
                table: "iml_applications",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "iml_is_importing_finished",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "iml_is_importing_in_bulk",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_conditions_for_control",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_good_manufacturing_practice",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "iml_another_activity",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "iml_is_importing_finished",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "iml_is_importing_in_bulk",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "is_conditions_for_control",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "is_good_manufacturing_practice",
                table: "iml_applications");

            migrationBuilder.AddColumn<string>(
                name: "iml_another_activity",
                table: "org_branches",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "iml_is_importing_finished",
                table: "org_branches",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "iml_is_importing_in_bulk",
                table: "org_branches",
                nullable: false,
                defaultValue: false);
        }
    }
}
