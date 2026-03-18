using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_Horus.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldsInMedicalRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChronicConditions",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ChronicConditions",
                table: "MedicalRecords");
        }
    }
}
