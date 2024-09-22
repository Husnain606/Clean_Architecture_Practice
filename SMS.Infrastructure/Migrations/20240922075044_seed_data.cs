using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SMS.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Administration", "ADMINISTRATION" },
                    { "2", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "CreatedBy", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "ModifiedAt", "ModifiedBy", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "ebdd8be7-54eb-4ead-baec-a926f6465195", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "hasnain606@gmail.com", true, false, false, null, null, null, "HASNAIN606@GMAIL.COM", "HUSNAINAHMED", "AQAAAAIAAYagAAAAEBsvg4p6iV67Gp2T2PooYHaDFjeoiB+V2FjNv+tDZPGQRajGvZUKhtTEcjPTcUI2Ug==", null, false, "998f803f-1a32-4ffa-b20b-4c5a3fb8fdfe", false, "HusnainAhmed" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "1" });
        }

  
    }
}
