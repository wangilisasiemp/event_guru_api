using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace event_guru_api.Migrations
{
    public partial class MigrationContributionstablechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConversationID",
                table: "Contributions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CustomerMSISDN",
                table: "Contributions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ResponseCode",
                table: "Contributions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ResponseDesc",
                table: "Contributions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ThirdPartyConversationID",
                table: "Contributions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TransactionID",
                table: "Contributions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationID",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "CustomerMSISDN",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "ResponseCode",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "ResponseDesc",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "ThirdPartyConversationID",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "TransactionID",
                table: "Contributions");
        }
    }
}
