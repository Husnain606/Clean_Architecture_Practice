using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigurationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95d308d0-845d-4c71-99a3-efee4f555b29", "AQAAAAIAAYagAAAAEPvC8U+q2nRlerY9bcBPyUmrEuUtMCsLeLZ8YHkg/TDQh3g5N2UB0DEKSjvMSMuBRw==", "72c7c565-42b4-4525-89aa-4ce331081d56" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ebdd8be7-54eb-4ead-baec-a926f6465195", "AQAAAAIAAYagAAAAEBsvg4p6iV67Gp2T2PooYHaDFjeoiB+V2FjNv+tDZPGQRajGvZUKhtTEcjPTcUI2Ug==", "998f803f-1a32-4ffa-b20b-4c5a3fb8fdfe" });
        }
    }
}
