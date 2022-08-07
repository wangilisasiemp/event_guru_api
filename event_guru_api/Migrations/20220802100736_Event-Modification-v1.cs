using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_guru_api.Migrations
{
    public partial class EventModificationv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_eventAttendances_AspNetUsers_AttendeeID",
                table: "eventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_eventAttendances_Events_EventID",
                table: "eventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_eventVendors_Events_EventID",
                table: "eventVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_eventVendors_Vendors_VendorID",
                table: "eventVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_eventVendors",
                table: "eventVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_eventAttendances",
                table: "eventAttendances");

            migrationBuilder.RenameTable(
                name: "eventVendors",
                newName: "EventVendors");

            migrationBuilder.RenameTable(
                name: "eventAttendances",
                newName: "EventAttendances");

            migrationBuilder.RenameColumn(
                name: "MinPrice",
                table: "Vendors",
                newName: "Silver");

            migrationBuilder.RenameColumn(
                name: "MaxPrice",
                table: "Vendors",
                newName: "Gold");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "EventVendors",
                newName: "BudgetID");

            migrationBuilder.RenameIndex(
                name: "IX_eventVendors_VendorID",
                table: "EventVendors",
                newName: "IX_EventVendors_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_eventAttendances_AttendeeID",
                table: "EventAttendances",
                newName: "IX_EventAttendances_AttendeeID");

            migrationBuilder.AddColumn<double>(
                name: "Bronze",
                table: "Vendors",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Vendors",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalizationDate",
                table: "Events",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfAttendees",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventVendors",
                table: "EventVendors",
                columns: new[] { "BudgetID", "VendorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventAttendances",
                table: "EventAttendances",
                columns: new[] { "EventID", "AttendeeID" });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BudgetType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MinValue = table.Column<double>(type: "double", nullable: true),
                    MaxValue = table.Column<double>(type: "double", nullable: true),
                    EventID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Budgets_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_EventID",
                table: "Budgets",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendances_AspNetUsers_AttendeeID",
                table: "EventAttendances",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendances_Events_EventID",
                table: "EventAttendances",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_AspNetUsers_AttendeeID",
                table: "EventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendances_Events_EventID",
                table: "EventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_EventVendors_Budgets_BudgetID",
                table: "EventVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_EventVendors_Vendors_VendorID",
                table: "EventVendors");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventVendors",
                table: "EventVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventAttendances",
                table: "EventAttendances");

            migrationBuilder.DropColumn(
                name: "Bronze",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "FinalizationDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NoOfAttendees",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "EventVendors",
                newName: "eventVendors");

            migrationBuilder.RenameTable(
                name: "EventAttendances",
                newName: "eventAttendances");

            migrationBuilder.RenameColumn(
                name: "Silver",
                table: "Vendors",
                newName: "MinPrice");

            migrationBuilder.RenameColumn(
                name: "Gold",
                table: "Vendors",
                newName: "MaxPrice");

            migrationBuilder.RenameColumn(
                name: "BudgetID",
                table: "eventVendors",
                newName: "EventID");

            migrationBuilder.RenameIndex(
                name: "IX_EventVendors_VendorID",
                table: "eventVendors",
                newName: "IX_eventVendors_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendances_AttendeeID",
                table: "eventAttendances",
                newName: "IX_eventAttendances_AttendeeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventVendors",
                table: "eventVendors",
                columns: new[] { "EventID", "VendorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventAttendances",
                table: "eventAttendances",
                columns: new[] { "EventID", "AttendeeID" });

            migrationBuilder.AddForeignKey(
                name: "FK_eventAttendances_AspNetUsers_AttendeeID",
                table: "eventAttendances",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_eventAttendances_Events_EventID",
                table: "eventAttendances",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_eventVendors_Events_EventID",
                table: "eventVendors",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_eventVendors_Vendors_VendorID",
                table: "eventVendors",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
