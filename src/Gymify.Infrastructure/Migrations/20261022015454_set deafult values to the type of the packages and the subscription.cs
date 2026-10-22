using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class setdeafultvaluestothetypeofthepackagesandthesubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "gym",
                table: "Subscriptions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Regular",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "gym",
                table: "Packages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Regular",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "gym",
                table: "Subscriptions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Regular");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "gym",
                table: "Packages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Regular");
        }
    }
}
