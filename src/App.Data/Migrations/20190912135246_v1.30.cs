using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v130 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "new_sgd_name",
                table: "messages",
                newName: "sgd_shief_old_full_name");

            migrationBuilder.AddColumn<Guid>(
                name: "old_location_id",
                table: "messages",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "sgd_old_full_name",
                table: "messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "old_location_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "sgd_old_full_name",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "sgd_shief_old_full_name",
                table: "messages",
                newName: "new_sgd_name");
        }
    }
}
