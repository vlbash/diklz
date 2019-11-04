using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v134 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "base_class",
                table: "result_input_controls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "result_input_controls",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "old_lims_id",
                table: "result_input_controls",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "org_unit_id",
                table: "result_input_controls",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "organization_info_id",
                table: "result_input_controls",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "parent_id",
                table: "result_input_controls",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "performer_id",
                table: "result_input_controls",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "reg_date",
                table: "result_input_controls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reg_number",
                table: "result_input_controls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_result_input_controls_org_unit_id",
                table: "result_input_controls",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_result_input_controls_parent_id",
                table: "result_input_controls",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_result_input_controls_performer_id",
                table: "result_input_controls",
                column: "performer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_result_input_controls_org_organization_org_unit_id",
                table: "result_input_controls",
                column: "org_unit_id",
                principalTable: "org_organization",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_result_input_controls_lims_docs_parent_id",
                table: "result_input_controls",
                column: "parent_id",
                principalTable: "lims_docs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_result_input_controls_org_employee_performer_id",
                table: "result_input_controls",
                column: "performer_id",
                principalTable: "org_employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_result_input_controls_org_organization_org_unit_id",
                table: "result_input_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_result_input_controls_lims_docs_parent_id",
                table: "result_input_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_result_input_controls_org_employee_performer_id",
                table: "result_input_controls");

            migrationBuilder.DropIndex(
                name: "ix_result_input_controls_org_unit_id",
                table: "result_input_controls");

            migrationBuilder.DropIndex(
                name: "ix_result_input_controls_parent_id",
                table: "result_input_controls");

            migrationBuilder.DropIndex(
                name: "ix_result_input_controls_performer_id",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "base_class",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "description",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "old_lims_id",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "org_unit_id",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "organization_info_id",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "performer_id",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "reg_date",
                table: "result_input_controls");

            migrationBuilder.DropColumn(
                name: "reg_number",
                table: "result_input_controls");
        }
    }
}
