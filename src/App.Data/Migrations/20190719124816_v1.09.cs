using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v109 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "iml_medicines",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    medicine_name = table.Column<string>(maxLength: 200, nullable: true),
                    form_name = table.Column<string>(nullable: true),
                    dose_in_unit = table.Column<string>(maxLength: 200, nullable: true),
                    number_of_units = table.Column<string>(maxLength: 100, nullable: true),
                    medicine_name_eng = table.Column<string>(nullable: true),
                    register_number = table.Column<string>(maxLength: 200, nullable: true),
                    atc_code = table.Column<string>(maxLength: 100, nullable: true),
                    producer_name = table.Column<string>(maxLength: 200, nullable: true),
                    producer_country = table.Column<string>(maxLength: 200, nullable: true),
                    supplier_name = table.Column<string>(maxLength: 200, nullable: true),
                    supplier_country = table.Column<string>(maxLength: 200, nullable: true),
                    supplier_address = table.Column<string>(nullable: true),
                    notes = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iml_medicines", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "iml_medicines");
        }
    }
}
