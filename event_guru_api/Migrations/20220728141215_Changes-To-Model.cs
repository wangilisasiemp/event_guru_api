using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_guru_api.Migrations
{
    public partial class ChangesToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contribution_AspNetUsers_AttendeeID",
                table: "Contribution");

            migrationBuilder.DropForeignKey(
                name: "FK_Contribution_Event_EventID",
                table: "Contribution");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_AspNetUsers_OrganizerID",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendance_AspNetUsers_AttendeeID",
                table: "EventAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_EventAttendance_Event_EventID",
                table: "EventAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_EventVendor_Event_EventID",
                table: "EventVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_EventVendor_Vendor_VendorID",
                table: "EventVendor");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_AttendeeID",
                table: "Invitation");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_Event_EventID",
                table: "Invitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventVendor",
                table: "EventVendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventAttendance",
                table: "EventAttendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contribution",
                table: "Contribution");

            migrationBuilder.RenameTable(
                name: "Vendor",
                newName: "Vendors");

            migrationBuilder.RenameTable(
                name: "Invitation",
                newName: "Invitations");

            migrationBuilder.RenameTable(
                name: "EventVendor",
                newName: "eventVendors");

            migrationBuilder.RenameTable(
                name: "EventAttendance",
                newName: "eventAttendances");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "Contribution",
                newName: "Contributions");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_EventID",
                table: "Invitations",
                newName: "IX_Invitations_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Invitation_AttendeeID",
                table: "Invitations",
                newName: "IX_Invitations_AttendeeID");

            migrationBuilder.RenameIndex(
                name: "IX_EventVendor_VendorID",
                table: "eventVendors",
                newName: "IX_eventVendors_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_EventAttendance_AttendeeID",
                table: "eventAttendances",
                newName: "IX_eventAttendances_AttendeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Event_OrganizerID",
                table: "Events",
                newName: "IX_Events_OrganizerID");

            migrationBuilder.RenameIndex(
                name: "IX_Contribution_EventID",
                table: "Contributions",
                newName: "IX_Contributions_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Contribution_AttendeeID",
                table: "Contributions",
                newName: "IX_Contributions_AttendeeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventVendors",
                table: "eventVendors",
                columns: new[] { "EventID", "VendorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_eventAttendances",
                table: "eventAttendances",
                columns: new[] { "EventID", "AttendeeID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contributions",
                table: "Contributions",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_AspNetUsers_AttendeeID",
                table: "Contributions",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Events_EventID",
                table: "Contributions",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID");

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
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events",
                column: "OrganizerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_AttendeeID",
                table: "Invitations",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Events_EventID",
                table: "Invitations",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_AspNetUsers_AttendeeID",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Events_EventID",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_eventAttendances_AspNetUsers_AttendeeID",
                table: "eventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_eventAttendances_Events_EventID",
                table: "eventAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerID",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_eventVendors_Events_EventID",
                table: "eventVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_eventVendors_Vendors_VendorID",
                table: "eventVendors");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_AttendeeID",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Events_EventID",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invitations",
                table: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_eventVendors",
                table: "eventVendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_eventAttendances",
                table: "eventAttendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contributions",
                table: "Contributions");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendor");

            migrationBuilder.RenameTable(
                name: "Invitations",
                newName: "Invitation");

            migrationBuilder.RenameTable(
                name: "eventVendors",
                newName: "EventVendor");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameTable(
                name: "eventAttendances",
                newName: "EventAttendance");

            migrationBuilder.RenameTable(
                name: "Contributions",
                newName: "Contribution");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_EventID",
                table: "Invitation",
                newName: "IX_Invitation_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Invitations_AttendeeID",
                table: "Invitation",
                newName: "IX_Invitation_AttendeeID");

            migrationBuilder.RenameIndex(
                name: "IX_eventVendors_VendorID",
                table: "EventVendor",
                newName: "IX_EventVendor_VendorID");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizerID",
                table: "Event",
                newName: "IX_Event_OrganizerID");

            migrationBuilder.RenameIndex(
                name: "IX_eventAttendances_AttendeeID",
                table: "EventAttendance",
                newName: "IX_EventAttendance_AttendeeID");

            migrationBuilder.RenameIndex(
                name: "IX_Contributions_EventID",
                table: "Contribution",
                newName: "IX_Contribution_EventID");

            migrationBuilder.RenameIndex(
                name: "IX_Contributions_AttendeeID",
                table: "Contribution",
                newName: "IX_Contribution_AttendeeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invitation",
                table: "Invitation",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventVendor",
                table: "EventVendor",
                columns: new[] { "EventID", "VendorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventAttendance",
                table: "EventAttendance",
                columns: new[] { "EventID", "AttendeeID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contribution",
                table: "Contribution",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contribution_AspNetUsers_AttendeeID",
                table: "Contribution",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contribution_Event_EventID",
                table: "Contribution",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_AspNetUsers_OrganizerID",
                table: "Event",
                column: "OrganizerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendance_AspNetUsers_AttendeeID",
                table: "EventAttendance",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventAttendance_Event_EventID",
                table: "EventAttendance",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventVendor_Event_EventID",
                table: "EventVendor",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventVendor_Vendor_VendorID",
                table: "EventVendor",
                column: "VendorID",
                principalTable: "Vendor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_AttendeeID",
                table: "Invitation",
                column: "AttendeeID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_Event_EventID",
                table: "Invitation",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
