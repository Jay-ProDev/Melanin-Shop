using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Melanin.Infrastructure.Database.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id_Category = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id_Category);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id_Member = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 320, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, defaultValue: "User"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id_Member);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id_Product = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    HairColor = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HairLength = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    HairTexture = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CapSize = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id_Product);
                    table.ForeignKey(
                        name: "FK_Product__Category",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id_Category",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id_Address = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MemberId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id_Address);
                    table.ForeignKey(
                        name: "FK_Address__Member",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id_Member",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id_CartItem = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Id_Product = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_Member = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id_CartItem);
                    table.ForeignKey(
                        name: "FK_CartItem__Member",
                        column: x => x.Id_Member,
                        principalTable: "Members",
                        principalColumn: "Id_Member",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem__Product",
                        column: x => x.Id_Product,
                        principalTable: "Product",
                        principalColumn: "Id_Product",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id_Order = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TotalPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CouponCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Id_Member = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_ShippingAddress = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_BillingAddress = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id_Order);
                    table.ForeignKey(
                        name: "FK_Order__BillingAddress",
                        column: x => x.Id_BillingAddress,
                        principalTable: "Address",
                        principalColumn: "Id_Address",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order__Member",
                        column: x => x.Id_Member,
                        principalTable: "Members",
                        principalColumn: "Id_Member",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order__ShippingAddress",
                        column: x => x.Id_ShippingAddress,
                        principalTable: "Address",
                        principalColumn: "Id_Address",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id_OrderItem = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Id_Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Id_Product = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id_OrderItem);
                    table.ForeignKey(
                        name: "FK_OrderItem__Order",
                        column: x => x.Id_Order,
                        principalTable: "Order",
                        principalColumn: "Id_Order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem__Product",
                        column: x => x.Id_Product,
                        principalTable: "Product",
                        principalColumn: "Id_Product",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id_Payment = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    StripeSessionId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Id_Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id_Payment);
                    table.ForeignKey(
                        name: "FK_Payment__Order",
                        column: x => x.Id_Order,
                        principalTable: "Order",
                        principalColumn: "Id_Order",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_MemberId",
                table: "Address",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_Id_Member",
                table: "CartItem",
                column: "Id_Member");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_Id_Product",
                table: "CartItem",
                column: "Id_Product");

            migrationBuilder.CreateIndex(
                name: "IDX_Category__slug",
                table: "Category",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_Member__email",
                table: "Members",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Id_BillingAddress",
                table: "Order",
                column: "Id_BillingAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Id_Member",
                table: "Order",
                column: "Id_Member");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Id_ShippingAddress",
                table: "Order",
                column: "Id_ShippingAddress");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_Id_Order",
                table: "OrderItem",
                column: "Id_Order");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_Id_Product",
                table: "OrderItem",
                column: "Id_Product");

            migrationBuilder.CreateIndex(
                name: "IDX_Payment__stripeSessionId",
                table: "Payment",
                column: "StripeSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Id_Order",
                table: "Payment",
                column: "Id_Order");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Members");
        }
    }
}
