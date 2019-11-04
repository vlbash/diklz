using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v132 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pharmacy_item_pharmacies",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    pharmacy_id = table.Column<Guid>(nullable: false),
                    pharmacy_item_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pharmacy_item_pharmacies", x => x.id);
                    table.ForeignKey(
                        name: "fk_pharmacy_item_pharmacies_org_branches_pharmacy_id",
                        column: x => x.pharmacy_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pharmacy_item_pharmacies_org_branches_pharmacy_item_id",
                        column: x => x.pharmacy_item_id,
                        principalTable: "org_branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_item_pharmacies_pharmacy_id",
                table: "pharmacy_item_pharmacies",
                column: "pharmacy_id");

            migrationBuilder.CreateIndex(
                name: "ix_pharmacy_item_pharmacies_pharmacy_item_id",
                table: "pharmacy_item_pharmacies",
                column: "pharmacy_item_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pharmacy_item_pharmacies");
        }
    }
}
