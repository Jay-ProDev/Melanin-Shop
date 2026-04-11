using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melanin.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Product_ProductId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItem");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartItem",
                newName: "Id_Product");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "CartItem",
                newName: "Id_Member");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItem",
                newName: "Id_CartItem");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItem",
                newName: "IX_CartItem_Id_Product");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_MemberId",
                table: "CartItem",
                newName: "IX_CartItem_Id_Member");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "CartItem",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "Id_CartItem")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem__Product",
                table: "CartItem",
                column: "Id_Product",
                principalTable: "Product",
                principalColumn: "Id_Product",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem__Product",
                table: "CartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.RenameTable(
                name: "CartItem",
                newName: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Id_Product",
                table: "CartItems",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Id_Member",
                table: "CartItems",
                newName: "MemberId");

            migrationBuilder.RenameColumn(
                name: "Id_CartItem",
                table: "CartItems",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_Id_Product",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_Id_Member",
                table: "CartItems",
                newName: "IX_CartItems_MemberId");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Product_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id_Product",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
