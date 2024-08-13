using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthwindBasedWebApplication.API.Migrations
{
    /// <inheritdoc />
    public partial class UserIdentificationColumnAddedToAspNetUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserIdentification",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIdentification",
                table: "AspNetUsers");
        }
    }
}
