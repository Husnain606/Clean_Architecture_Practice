using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class updateStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentLastName",
                table: "Student",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "StudentFirstName",
                table: "Student",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "StudentFatherName",
                table: "Student",
                newName: "FatherName");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95f2f4c8-b97f-418d-bf92-1ba4b69db1e6", "AQAAAAIAAYagAAAAENEDad6OfDcPlM1a3xvSWsdacEuB2XhIeMp1gQQ+npY779IUyiQEHiYw+QL8Kuf5Sg==", "ca3f8958-158b-41b5-97af-1e958724fb64" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Student",
                newName: "StudentLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Student",
                newName: "StudentFirstName");

            migrationBuilder.RenameColumn(
                name: "FatherName",
                table: "Student",
                newName: "StudentFatherName");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "302af87b-4d07-4de5-b207-1beed39e5f91", "AQAAAAIAAYagAAAAEP3rmtkrGrl3J15x1CSQQMb2aPqMfz78Uzo09QoVvjZKYeNh7dbWsd4SC0iEYAbN7g==", "4db94f94-32cf-4374-b405-33eecd051447" });
        }
    }
}
