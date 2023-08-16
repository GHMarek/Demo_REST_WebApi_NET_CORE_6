using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoRESTWebApi.Migrations
{
    /// <inheritdoc />
    public partial class ResourceOperationauthorize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Libraries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_CreatedById",
                table: "Libraries",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Users_CreatedById",
                table: "Libraries",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Users_CreatedById",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Libraries_CreatedById",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Libraries");
        }
    }
}
