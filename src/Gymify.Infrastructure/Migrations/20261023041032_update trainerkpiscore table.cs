using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetrainerkpiscoretable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainerKpiScores_TrainerId_EvaluationDate",
                schema: "staff",
                table: "TrainerKpiScores");

            migrationBuilder.DropIndex(
                name: "IX_TrainerKpiScores_TrainerId_KpiId_EvaluationDate",
                schema: "staff",
                table: "TrainerKpiScores");

            migrationBuilder.DropColumn(
                name: "EvaluationDate",
                schema: "staff",
                table: "TrainerKpiScores");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                schema: "staff",
                table: "TrainerKpiScores",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_TrainerKpiScores_TrainerId_Date",
                schema: "staff",
                table: "TrainerKpiScores",
                columns: new[] { "TrainerId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerKpiScores_TrainerId_KpiId_Date",
                schema: "staff",
                table: "TrainerKpiScores",
                columns: new[] { "TrainerId", "KpiId", "Date" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainerKpiScores_TrainerId_Date",
                schema: "staff",
                table: "TrainerKpiScores");

            migrationBuilder.DropIndex(
                name: "IX_TrainerKpiScores_TrainerId_KpiId_Date",
                schema: "staff",
                table: "TrainerKpiScores");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "staff",
                table: "TrainerKpiScores");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EvaluationDate",
                schema: "staff",
                table: "TrainerKpiScores",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_TrainerKpiScores_TrainerId_EvaluationDate",
                schema: "staff",
                table: "TrainerKpiScores",
                columns: new[] { "TrainerId", "EvaluationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerKpiScores_TrainerId_KpiId_EvaluationDate",
                schema: "staff",
                table: "TrainerKpiScores",
                columns: new[] { "TrainerId", "KpiId", "EvaluationDate" });
        }
    }
}
