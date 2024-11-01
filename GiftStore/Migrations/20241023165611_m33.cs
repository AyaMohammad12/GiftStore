using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftStore.Migrations
{
    /// <inheritdoc />
    public partial class m33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "cartItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_cartItems_UserId",
                table: "cartItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_cartItems_AspNetUsers_UserId",
                table: "cartItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cartItems_AspNetUsers_UserId",
                table: "cartItems");

            migrationBuilder.DropIndex(
                name: "IX_cartItems_UserId",
                table: "cartItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "cartItems");
        }
    }
}
