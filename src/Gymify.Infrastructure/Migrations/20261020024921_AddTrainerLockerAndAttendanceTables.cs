using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerLockerAndAttendanceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                schema: "staff",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lockers",
                schema: "gym",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_Lockers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                schema: "gym",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LockerId = table.Column<int>(type: "int", nullable: false),
                    CheckInAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CheckOutAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
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
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "users",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_Lockers_LockerId",
                        column: x => x.LockerId,
                        principalSchema: "gym",
                        principalTable: "Lockers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Code",
                schema: "users",
                table: "Customers",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CustomerId",
                schema: "gym",
                table: "Attendances",
                column: "CustomerId",
                unique: true,
                filter: "[CheckOutAt] IS NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LockerId",
                schema: "gym",
                table: "Attendances",
                column: "LockerId",
                unique: true,
                filter: "[CheckOutAt] IS NULL AND [IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Lockers_Number",
                schema: "gym",
                table: "Lockers",
                column: "Number",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances",
                schema: "gym");

            migrationBuilder.DropTable(
                name: "Lockers",
                schema: "gym");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Code",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                schema: "staff",
                table: "Trainers");
        }
    }
}
