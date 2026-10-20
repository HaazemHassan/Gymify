using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DomainUsers_ApprovalStatus",
                schema: "users",
                table: "DomainUsers");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                schema: "users",
                table: "DomainUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                schema: "users",
                table: "DomainUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainUserId = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_DomainUsers_DomainUserId",
                        column: x => x.DomainUserId,
                        principalSchema: "users",
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_DomainUserId",
                schema: "users",
                table: "Customers",
                column: "DomainUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers",
                schema: "users");

            migrationBuilder.DropColumn(
                name: "UserType",
                schema: "users",
                table: "DomainUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                schema: "users",
                table: "DomainUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_ApprovalStatus",
                schema: "users",
                table: "DomainUsers",
                column: "ApprovalStatus");
        }
    }
}
