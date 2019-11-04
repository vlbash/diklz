using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v114 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "lims_rp_id",
                table: "iml_medicines",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_iml_medicines_lims_rp_id",
                table: "iml_medicines",
                column: "lims_rp_id");

            migrationBuilder.AddForeignKey(
                name: "fk_iml_medicines_lims_rp_lims_rp_id",
                table: "iml_medicines",
                column: "lims_rp_id",
                principalTable: "lims_rp",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iml_medicines_lims_rp_lims_rp_id",
                table: "iml_medicines");

            migrationBuilder.DropIndex(
                name: "ix_iml_medicines_lims_rp_id",
                table: "iml_medicines");

            migrationBuilder.DropColumn(
                name: "lims_rp_id",
                table: "iml_medicines");
        }
    }
}
