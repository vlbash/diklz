using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "entity_id",
                table: "edocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "entity_name",
                table: "edocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "entity_id",
                table: "edocuments");

            migrationBuilder.DropColumn(
                name: "entity_name",
                table: "edocuments");
        }
    }
}
