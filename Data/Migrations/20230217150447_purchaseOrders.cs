using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class purchaseOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    PurchaseID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VendorID = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.PurchaseID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Vendor_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendor",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<uint>(type: "INTEGER", nullable: false),
                    PurchaseOrderPurchaseID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Item_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_PurchaseOrder_PurchaseOrderPurchaseID",
                        column: x => x.PurchaseOrderPurchaseID,
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_ItemID",
                table: "PurchaseItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_PurchaseOrderPurchaseID",
                table: "PurchaseItems",
                column: "PurchaseOrderPurchaseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_VendorID",
                table: "PurchaseOrder",
                column: "VendorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseItems");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");
        }
    }
}
