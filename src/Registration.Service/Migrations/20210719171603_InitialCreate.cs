using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Registration.Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrationStateInstance",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(nullable: false),
                    ParticipantEmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    ParticipantLicenseNumber = table.Column<string>(maxLength: 20, nullable: true),
                    ParticipantCategory = table.Column<string>(maxLength: 20, nullable: true),
                    EventId = table.Column<string>(maxLength: 60, nullable: true),
                    RaceId = table.Column<string>(maxLength: 60, nullable: true),
                    CurrentState = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationStateInstance", x => x.CorrelationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationStateInstance");
        }
    }
}
