using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registration.Service.Migrations
{
    /// <inheritdoc />
    public partial class Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ParticipantLicenseExpirationDate",
                table: "RegistrationState",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantLicenseExpirationDate",
                table: "RegistrationState");
        }
    }
}
