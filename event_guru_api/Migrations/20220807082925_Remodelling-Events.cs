using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_guru_api.Migrations
{
    public partial class RemodellingEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bronze",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Vendors");

            migrationBuilder.RenameColumn(
                name: "Silver",
                table: "Vendors",
                newName: "Price");

            migrationBuilder.AddColumn<bool>(
                name: "Caterer",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ConferenceHall",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Decoration",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Drinks",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Entertainment",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "EventBudget",
                table: "Events",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MC",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OrdinaryTransport",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RoyalTransport",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Security",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caterer",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ConferenceHall",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Decoration",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Drinks",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Entertainment",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventBudget",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MC",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrdinaryTransport",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RoyalTransport",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Security",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Vendors",
                newName: "Silver");

            migrationBuilder.AddColumn<double>(
                name: "Bronze",
                table: "Vendors",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Gold",
                table: "Vendors",
                type: "double",
                nullable: true);
        }
    }
}
