using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalityMatrixApp.DataAccess.Migrations
{
    public partial class AttachmentEntityChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Attachments",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Attachments",
                newName: "Url");
        }
    }
}
