using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pharmacy_head_full_name",
                table: "messages");

            migrationBuilder.AddColumn<string>(
                name: "pharmacy_head_last_name",
                table: "messages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pharmacy_head_middle_name",
                table: "messages",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pharmacy_head_name",
                table: "messages",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pharmacy_head_last_name",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "pharmacy_head_middle_name",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "pharmacy_head_name",
                table: "messages");

            migrationBuilder.AddColumn<string>(
                name: "pharmacy_head_full_name",
                table: "messages",
                nullable: true);
        }
    }
}
