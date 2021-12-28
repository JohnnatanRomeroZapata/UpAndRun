using Microsoft.EntityFrameworkCore.Migrations;

namespace UpAndRun.Migrations
{
    public partial class UpdateTableApplicationType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ApplicationTypes",
                newName: "ApplicationName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApplicationName",
                table: "ApplicationTypes",
                newName: "Name");
        }
    }
}
