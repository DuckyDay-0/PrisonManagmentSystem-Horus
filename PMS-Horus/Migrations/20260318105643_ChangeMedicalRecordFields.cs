using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_Horus.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMedicalRecordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ChronicConditions",
                table: "MedicalRecords");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Allergies",
                table: "MedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChronicConditions",
                table: "MedicalRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
