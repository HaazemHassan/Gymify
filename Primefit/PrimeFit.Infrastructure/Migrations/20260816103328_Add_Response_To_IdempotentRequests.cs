using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Response_To_IdempotentRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Response",
                schema: "idempotency",
                table: "IdempotentRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseType",
                schema: "idempotency",
                table: "IdempotentRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Response",
                schema: "idempotency",
                table: "IdempotentRequests");

            migrationBuilder.DropColumn(
                name: "ResponseType",
                schema: "idempotency",
                table: "IdempotentRequests");
        }
    }
}
