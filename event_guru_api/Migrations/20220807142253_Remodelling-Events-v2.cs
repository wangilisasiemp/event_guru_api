using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_guru_api.Migrations
{
    public partial class RemodellingEventsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "MinValue",
                table: "Budgets",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Budgets",
                newName: "MinValue");

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "Budgets",
                type: "double",
                nullable: true);
        }
    }
}
