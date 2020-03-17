using Microsoft.EntityFrameworkCore.Migrations;

namespace BattlestaHealthChecks.Migrations
{
    public partial class another : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CredentialPath",
                table: "BackupSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CredentialPath",
                table: "BackupSettings");
        }
    }
}
