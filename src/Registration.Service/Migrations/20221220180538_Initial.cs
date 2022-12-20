using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registration.Service.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParticipantLicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParticipantCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationState", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationState");
        }
    }
}
