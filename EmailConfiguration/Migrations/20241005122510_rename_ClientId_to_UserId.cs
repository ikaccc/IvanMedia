using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailConfiguration.Migrations
{
    /// <inheritdoc />
    public partial class rename_ClientId_to_UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "EmailConfigurations",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmailConfigurations",
                newName: "ClientId");
        }
    }
}
