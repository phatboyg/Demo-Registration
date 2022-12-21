using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registration.Service.Migrations
{
    /// <inheritdoc />
    public partial class RegistrationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationId",
                table: "RegistrationState",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "RegistrationState");
        }
    }
}
