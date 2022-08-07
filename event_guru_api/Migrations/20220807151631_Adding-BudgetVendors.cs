using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_guru_api.Migrations
{
    public partial class AddingBudgetVendors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventVendors_Budgets_BudgetID",
                table: "EventVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_EventVendors_Vendors_VendorID",
                table: "EventVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventVendors",
                table: "EventVendors");

            migrationBuilder.RenameTable(
                name: "EventVendors",
                newName: "BudgetVendors");

            migrationBuilder.RenameIndex(
                name: "IX_EventVendors_VendorID",
                table: "BudgetVendors",
                newName: "IX_BudgetVendors_VendorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetVendors",
                table: "BudgetVendors",
                columns: new[] { "BudgetID", "VendorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetVendors_Budgets_BudgetID",
                table: "BudgetVendors",
                column: "BudgetID",
                principalTable: "Budgets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetVendors_Vendors_VendorID",
                table: "BudgetVendors",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetVendors_Budgets_BudgetID",
                table: "BudgetVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetVendors_Vendors_VendorID",
                table: "BudgetVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetVendors",
                table: "BudgetVendors");

            migrationBuilder.RenameTable(
                name: "BudgetVendors",
                newName: "EventVendors");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetVendors_VendorID",
                table: "EventVendors",
                newName: "IX_EventVendors_VendorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventVendors",
                table: "EventVendors",
                columns: new[] { "BudgetID", "VendorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventVendors_Budgets_BudgetID",
                table: "EventVendors",
                column: "BudgetID",
                principalTable: "Budgets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventVendors_Vendors_VendorID",
                table: "EventVendors",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
