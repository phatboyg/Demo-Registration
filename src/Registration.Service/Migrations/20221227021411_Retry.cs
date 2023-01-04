using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registration.Service.Migrations
{
    /// <inheritdoc />
    public partial class Retry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "RegistrationState",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "RegistrationState",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryAttempt",
                table: "RegistrationState",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleRetryToken",
                table: "RegistrationState",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "RegistrationState");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "RegistrationState");

            migrationBuilder.DropColumn(
                name: "RetryAttempt",
                table: "RegistrationState");

            migrationBuilder.DropColumn(
                name: "ScheduleRetryToken",
                table: "RegistrationState");
        }
    }
}
