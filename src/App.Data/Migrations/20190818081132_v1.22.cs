using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iml_applications_departmental_subordinations_departmental_s~",
                table: "iml_applications");

            migrationBuilder.DropForeignKey(
                name: "fk_prl_applications_departmental_subordinations_departmental_s~",
                table: "prl_applications");

            migrationBuilder.DropForeignKey(
                name: "fk_trl_applications_departmental_subordinations_departmental_s~",
                table: "trl_applications");

            migrationBuilder.DropIndex(
                name: "ix_trl_applications_departmental_subordination_id",
                table: "trl_applications");

            migrationBuilder.DropIndex(
                name: "ix_prl_applications_departmental_subordination_id",
                table: "prl_applications");

            migrationBuilder.DropIndex(
                name: "ix_iml_applications_departmental_subordination_id",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "departmental_subordination_id",
                table: "trl_applications");

            migrationBuilder.DropColumn(
                name: "departmental_subordination_id",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "departmental_subordination_id",
                table: "iml_applications");

            migrationBuilder.AddColumn<bool>(
                name: "prl_in_pharmacies",
                table: "prl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "retail_of_medicines",
                table: "prl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "wholesale_of_medicines",
                table: "prl_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "prl_in_pharmacies",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "retail_of_medicines",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "wholesale_of_medicines",
                table: "iml_applications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "prl_in_pharmacies",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "retail_of_medicines",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "wholesale_of_medicines",
                table: "prl_applications");

            migrationBuilder.DropColumn(
                name: "prl_in_pharmacies",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "retail_of_medicines",
                table: "iml_applications");

            migrationBuilder.DropColumn(
                name: "wholesale_of_medicines",
                table: "iml_applications");

            migrationBuilder.AddColumn<Guid>(
                name: "departmental_subordination_id",
                table: "trl_applications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "departmental_subordination_id",
                table: "prl_applications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "departmental_subordination_id",
                table: "iml_applications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_trl_applications_departmental_subordination_id",
                table: "trl_applications",
                column: "departmental_subordination_id");

            migrationBuilder.CreateIndex(
                name: "ix_prl_applications_departmental_subordination_id",
                table: "prl_applications",
                column: "departmental_subordination_id");

            migrationBuilder.CreateIndex(
                name: "ix_iml_applications_departmental_subordination_id",
                table: "iml_applications",
                column: "departmental_subordination_id");

            migrationBuilder.AddForeignKey(
                name: "fk_iml_applications_departmental_subordinations_departmental_s~",
                table: "iml_applications",
                column: "departmental_subordination_id",
                principalTable: "departmental_subordinations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_prl_applications_departmental_subordinations_departmental_s~",
                table: "prl_applications",
                column: "departmental_subordination_id",
                principalTable: "departmental_subordinations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_trl_applications_departmental_subordinations_departmental_s~",
                table: "trl_applications",
                column: "departmental_subordination_id",
                principalTable: "departmental_subordinations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
