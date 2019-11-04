using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_pending_change_info",
                table: "org_organization_info",
                newName: "is_pending_license_update");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_pending_license_update",
                table: "org_organization_info",
                newName: "is_pending_change_info");
        }
    }
}
