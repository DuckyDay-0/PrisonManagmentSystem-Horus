using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_Horus.Migrations
{
    /// <inheritdoc />
    public partial class addFirstNameLastName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Prisoners",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Prisoners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Prisoners");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Prisoners",
                newName: "Name");
        }
    }
}
