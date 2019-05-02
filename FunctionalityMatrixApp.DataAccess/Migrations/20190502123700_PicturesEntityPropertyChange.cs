using Microsoft.EntityFrameworkCore.Migrations;

namespace FunctionalityMatrixApp.DataAccess.Migrations
{
    public partial class PicturesEntityPropertyChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Pictures",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Pictures",
                newName: "Url");
        }
    }
}
