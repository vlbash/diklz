using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v113 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iml_medicines_atu_country_producer_country_id",
                table: "iml_medicines");

            migrationBuilder.DropForeignKey(
                name: "fk_iml_medicines_atu_country_supplier_country_id",
                table: "iml_medicines");

            migrationBuilder.DropIndex(
                name: "ix_iml_medicines_producer_country_id",
                table: "iml_medicines");

            migrationBuilder.DropIndex(
                name: "ix_iml_medicines_supplier_country_id",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "producer_country_id",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "supplier_country_id",
                table: "iml_medicines");

            migrationBuilder.AddColumn<string>(
                name: "producer_country",
                table: "iml_medicines",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "supplier_country",
                table: "iml_medicines",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "producer_country",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "supplier_country",
                table: "iml_medicines");

            migrationBuilder.AddColumn<Guid>(
                name: "producer_country_id",
                table: "iml_medicines",
                maxLength: 200,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "supplier_country_id",
                table: "iml_medicines",
                maxLength: 200,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_iml_medicines_producer_country_id",
                table: "iml_medicines",
                column: "producer_country_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_medicines_supplier_country_id",
                table: "iml_medicines",
                column: "supplier_country_id");

            migrationBuilder.AddForeignKey(
                name: "fk_iml_medicines_atu_country_producer_country_id",
                table: "iml_medicines",
                column: "producer_country_id",
                principalTable: "atu_country",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_iml_medicines_atu_country_supplier_country_id",
                table: "iml_medicines",
                column: "supplier_country_id",
                principalTable: "atu_country",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
