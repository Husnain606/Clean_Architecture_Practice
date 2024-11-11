using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class updateTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherLastName",
                table: "Teacher",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "TeacherFirstName",
                table: "Teacher",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "TeacherFatherName",
                table: "Teacher",
                newName: "FatherName");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e463ee71-2ae5-4fc1-932a-f37399176ec4", "AQAAAAIAAYagAAAAEK55QG4ruVRr2tVQlVtd49An0NRsT09zQjrayi4Jeq1YJThdOAvP8WHV5O+ethJNug==", "09fe7fb8-5b8f-4d61-a0f8-a3aca92a98dd" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Teacher",
                newName: "TeacherLastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Teacher",
                newName: "TeacherFirstName");

            migrationBuilder.RenameColumn(
                name: "FatherName",
                table: "Teacher",
                newName: "TeacherFatherName");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95f2f4c8-b97f-418d-bf92-1ba4b69db1e6", "AQAAAAIAAYagAAAAENEDad6OfDcPlM1a3xvSWsdacEuB2XhIeMp1gQQ+npY779IUyiQEHiYw+QL8Kuf5Sg==", "ca3f8958-158b-41b5-97af-1e958724fb64" });
        }
    }
}
