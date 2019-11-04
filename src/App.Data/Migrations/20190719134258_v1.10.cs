using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v110 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "application_id",
                table: "iml_medicines",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "iml_application_id",
                table: "iml_medicines",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_iml_medicines_iml_application_id",
                table: "iml_medicines",
                column: "iml_application_id");

            migrationBuilder.AddForeignKey(
                name: "fk_iml_medicines_iml_applications_iml_application_id",
                table: "iml_medicines",
                column: "iml_application_id",
                principalTable: "iml_applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iml_medicines_iml_applications_iml_application_id",
                table: "iml_medicines");

            migrationBuilder.DropIndex(
                name: "ix_iml_medicines_iml_application_id",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "application_id",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "iml_application_id",
                table: "iml_medicines");
        }
    }
}
