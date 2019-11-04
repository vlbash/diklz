using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v140 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_conclusions",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    parent_id = table.Column<Guid>(nullable: true),
                    performer_id = table.Column<Guid>(nullable: true),
                    reg_number = table.Column<string>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    org_unit_id = table.Column<Guid>(nullable: false),
                    organization_info_id = table.Column<Guid>(nullable: false),
                    old_lims_id = table.Column<long>(nullable: false),
                    base_class = table.Column<string>(nullable: true),
                    branch_id = table.Column<Guid>(nullable: false),
                    app_conclusion_status = table.Column<string>(nullable: true),
                    app_state = table.Column<string>(nullable: true),
                    app_sort = table.Column<string>(nullable: true),
                    doc_num = table.Column<string>(nullable: true),
                    app_reg_date = table.Column<DateTime>(nullable: true),
                    assigne = table.Column<string>(nullable: true),
                    teritorial_service = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_conclusions", x => x.id);
                    table.ForeignKey(
                        name: "fk_app_conclusions_org_branches_branch_id",
                        column: x => x.branch_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_conclusions_org_organization_org_unit_id",
                        column: x => x.org_unit_id,
                        principalTable: "org_organization",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_conclusions_lims_docs_parent_id",
                        column: x => x.parent_id,
                        principalTable: "lims_docs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_app_conclusions_org_employee_performer_id",
                        column: x => x.performer_id,
                        principalTable: "org_employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_conclusions_branch_id",
                table: "app_conclusions",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_conclusions_org_unit_id",
                table: "app_conclusions",
                column: "org_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_conclusions_parent_id",
                table: "app_conclusions",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_conclusions_performer_id",
                table: "app_conclusions",
                column: "performer_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_conclusions");
        }
    }
}
