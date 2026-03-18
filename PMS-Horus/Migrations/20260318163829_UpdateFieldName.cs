using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_Horus.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Prisoners_PrisonerID",
                table: "MedicalRecords");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Prisoners",
                newName: "PrisonerId");

            migrationBuilder.RenameColumn(
                name: "PrisonerID",
                table: "MedicalRecords",
                newName: "PrisonerId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_PrisonerID",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_PrisonerId");

            migrationBuilder.CreateIndex(
                name: "IX_Prisoners_PersonalIDNumber",
                table: "Prisoners",
                column: "PersonalIDNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Prisoners_PrisonerId",
                table: "MedicalRecords",
                column: "PrisonerId",
                principalTable: "Prisoners",
                principalColumn: "PrisonerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Prisoners_PrisonerId",
                table: "MedicalRecords");

            migrationBuilder.DropIndex(
                name: "IX_Prisoners_PersonalIDNumber",
                table: "Prisoners");

            migrationBuilder.RenameColumn(
                name: "PrisonerId",
                table: "Prisoners",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PrisonerId",
                table: "MedicalRecords",
                newName: "PrisonerID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_PrisonerId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_PrisonerID");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Prisoners_PrisonerID",
                table: "MedicalRecords",
                column: "PrisonerID",
                principalTable: "Prisoners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
