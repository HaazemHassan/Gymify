using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class allowthepackageandsubscriptiontohavemultipletypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                schema: "gym",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "gym",
                table: "Subscriptions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                schema: "gym",
                table: "Packages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "gym",
                table: "Packages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TrainerId",
                schema: "gym",
                table: "Subscriptions",
                column: "TrainerId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Subscriptions_Type_TrainerId",
                schema: "gym",
                table: "Subscriptions",
                sql: "([Type] = 'Private' AND [TrainerId] IS NOT NULL) OR ([Type] <> 'Private' AND [TrainerId] IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_TrainerId",
                schema: "gym",
                table: "Packages",
                column: "TrainerId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Packages_Type_TrainerId",
                schema: "gym",
                table: "Packages",
                sql: "([Type] = 'Private' AND [TrainerId] IS NOT NULL) OR ([Type] <> 'Private' AND [TrainerId] IS NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Trainers_TrainerId",
                schema: "gym",
                table: "Packages",
                column: "TrainerId",
                principalSchema: "staff",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Trainers_TrainerId",
                schema: "gym",
                table: "Subscriptions",
                column: "TrainerId",
                principalSchema: "staff",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Trainers_TrainerId",
                schema: "gym",
                table: "Packages");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Trainers_TrainerId",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_TrainerId",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Subscriptions_Type_TrainerId",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Packages_TrainerId",
                schema: "gym",
                table: "Packages");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Packages_Type_TrainerId",
                schema: "gym",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "gym",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                schema: "gym",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "gym",
                table: "Packages");
        }
    }
}
