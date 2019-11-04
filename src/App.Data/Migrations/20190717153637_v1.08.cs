using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Data.Migrations
{
    public partial class v108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lims_rp",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    record_state = table.Column<int>(nullable: false),
                    caption = table.Column<string>(maxLength: 128, nullable: true),
                    modified_by = table.Column<Guid>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: true),
                    created_by = table.Column<Guid>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    doc_id = table.Column<int>(nullable: false),
                    reg_num = table.Column<string>(nullable: true),
                    reg_proc_code = table.Column<string>(nullable: true),
                    state_id = table.Column<int>(nullable: true),
                    reg_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true),
                    ord_reg_num = table.Column<string>(nullable: true),
                    ord_reg_date = table.Column<DateTime>(nullable: true),
                    drug_name_ukr = table.Column<string>(nullable: true),
                    drug_name_eng = table.Column<string>(nullable: true),
                    form_type_id = table.Column<int>(nullable: true),
                    formtype_desc = table.Column<string>(nullable: true),
                    form_name = table.Column<string>(nullable: true),
                    farm_group = table.Column<string>(nullable: true),
                    side_name = table.Column<string>(nullable: true),
                    country_id = table.Column<int>(nullable: true),
                    country_name = table.Column<string>(nullable: true),
                    producer_name = table.Column<string>(nullable: true),
                    prod_country_name = table.Column<string>(nullable: true),
                    is_resident = table.Column<bool>(nullable: false),
                    reg_procedure = table.Column<string>(nullable: true),
                    regproc_id = table.Column<int>(nullable: true),
                    regproc_name = table.Column<string>(nullable: true),
                    regproc_code = table.Column<string>(nullable: true),
                    drugtype_id = table.Column<int>(nullable: true),
                    drugtype_name = table.Column<string>(nullable: true),
                    rp_order_id = table.Column<int>(nullable: true),
                    off_order_num = table.Column<string>(nullable: true),
                    off_order_date = table.Column<DateTime>(nullable: true),
                    off_reason = table.Column<string>(nullable: true),
                    drug_class_id = table.Column<int>(nullable: true),
                    drug_class_name = table.Column<string>(nullable: true),
                    atc_code = table.Column<string>(nullable: true),
                    active_substances = table.Column<string>(nullable: true),
                    sale_terms = table.Column<string>(nullable: true),
                    publicity_info = table.Column<string>(nullable: true),
                    notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lims_rp", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lims_rp");
        }
    }
}
