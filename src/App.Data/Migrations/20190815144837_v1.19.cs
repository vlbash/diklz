using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v119 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "departmental_subordinations",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departmental_subordinations", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "departmental_subordinations");

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
        }
    }
}
