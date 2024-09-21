using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeacherLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeacherFatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pasword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfirmPasword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HiringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.Id);
                });

          
            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Mail",
                table: "Teacher",
                column: "Mail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(
                name: "Teacher");

           
        }
    }
}
