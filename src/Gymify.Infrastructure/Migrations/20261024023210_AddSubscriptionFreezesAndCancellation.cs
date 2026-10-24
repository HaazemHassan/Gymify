using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionFreezesAndCancellation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                schema: "gym",
                table: "Subscriptions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CancelledAt",
                schema: "gym",
                table: "Subscriptions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                schema: "gym",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SubscriptionFreezes",
                schema: "gym",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_SubscriptionFreezes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionFreezes_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalSchema: "gym",
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_IsCancelled",
                schema: "gym",
                table: "Subscriptions",
                column: "IsCancelled");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionFreezes_SubscriptionId_StartDate_EndDate",
                schema: "gym",
                table: "SubscriptionFreezes",
                columns: new[] { "SubscriptionId", "StartDate", "EndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionFreezes",
                schema: "gym");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_IsCancelled",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                schema: "gym",
                table: "Subscriptions");
        }
    }
}
