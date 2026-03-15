using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_Horus.Migrations
{
    /// <inheritdoc />
    public partial class AddingPersonalIDNumberField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonalIDNumber",
                table: "Prisoners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalIDNumber",
                table: "Prisoners");
        }
    }
}
