using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionDatesAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_CustomerId",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDate",
                schema: "gym",
                table: "Subscriptions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                schema: "gym",
                table: "Subscriptions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId_EndDate",
                schema: "gym",
                table: "Subscriptions",
                columns: new[] { "CustomerId", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ActiveOccupancy",
                schema: "gym",
                table: "Attendances",
                column: "Id",
                filter: "[CheckOutAt] IS NULL AND [IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_CustomerId_EndDate",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ActiveOccupancy",
                schema: "gym",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId",
                schema: "gym",
                table: "Subscriptions",
                column: "CustomerId");
        }
    }
}
