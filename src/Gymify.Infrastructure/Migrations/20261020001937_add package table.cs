using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addpackagetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "staff");

            migrationBuilder.EnsureSchema(
                name: "gym");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                schema: "users",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChronicDiseases",
                schema: "users",
                table: "Customers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "users",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FatPercentage",
                schema: "users",
                table: "Customers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Injuries",
                schema: "users",
                table: "Customers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MusclePercentage",
                schema: "users",
                table: "Customers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "users",
                table: "Customers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainUserId = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_DomainUsers_DomainUserId",
                        column: x => x.DomainUserId,
                        principalSchema: "users",
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                schema: "gym",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                schema: "staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "staff",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                schema: "gym",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PackagePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PackageDuration = table.Column<int>(type: "int", nullable: false),
                    PackageDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "users",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Packages_PackageId",
                        column: x => x.PackageId,
                        principalSchema: "gym",
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DomainUserId",
                schema: "staff",
                table: "Employees",
                column: "DomainUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId",
                schema: "gym",
                table: "Subscriptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PackageId",
                schema: "gym",
                table: "Subscriptions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_EmployeeId",
                schema: "staff",
                table: "Trainers",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions",
                schema: "gym");

            migrationBuilder.DropTable(
                name: "Trainers",
                schema: "staff");

            migrationBuilder.DropTable(
                name: "Packages",
                schema: "gym");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "staff");

            migrationBuilder.DropColumn(
                name: "Age",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ChronicDiseases",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FatPercentage",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Injuries",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "MusclePercentage",
                schema: "users",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "users",
                table: "Customers");
        }
    }
}
