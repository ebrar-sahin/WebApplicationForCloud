using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations.WarehouseDb
{
    /// <inheritdoc />
    public partial class AddValidations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { /*
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Customer_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Postal_Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__8CB286B9D5011196", x => x.Customer_ID);
                });

            migrationBuilder.CreateTable(
                name: "Subcontractor",
                columns: table => new
                {
                    Subcontractor_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Postal_Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subcontr__08DBCF261B53DB3A", x => x.Subcontractor_ID);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Event_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address_ID = table.Column<int>(type: "int", nullable: true),
                    Customer_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Event__FD6BEFE4A36DB87F", x => x.Event_ID);
                    table.ForeignKey(
                        name: "FK__Event__Customer___6E01572D",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Customer_ID");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Invoice_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Due_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Customer_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Invoice__0DE60494BD2234AD", x => x.Invoice_ID);
                    table.ForeignKey(
                        name: "FK__Invoice__Custome__70DDC3D8",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Customer_ID");
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Material_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Material_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    Subcontractor_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__3A09B0FDC87BF539", x => x.Material_ID);
                    table.ForeignKey(
                        name: "FK__Material__Subcon__73BA3083",
                        column: x => x.Subcontractor_ID,
                        principalTable: "Subcontractor",
                        principalColumn: "Subcontractor_ID");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Product_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    Availability = table.Column<bool>(type: "bit", nullable: true),
                    Material_ID = table.Column<int>(type: "int", nullable: true),
                    Subcontractor_ID = table.Column<int>(type: "int", nullable: true),
                    Invoice_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__9834FB9A8A0B853C", x => x.Product_ID);
                    table.ForeignKey(
                        name: "FK__Product__Invoice__787EE5A0",
                        column: x => x.Invoice_ID,
                        principalTable: "Invoice",
                        principalColumn: "Invoice_ID");
                    table.ForeignKey(
                        name: "FK__Product__Materia__76969D2E",
                        column: x => x.Material_ID,
                        principalTable: "Material",
                        principalColumn: "Material_ID");
                    table.ForeignKey(
                        name: "FK__Product__Subcont__778AC167",
                        column: x => x.Subcontractor_ID,
                        principalTable: "Subcontractor",
                        principalColumn: "Subcontractor_ID");
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Log_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product_ID = table.Column<int>(type: "int", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Log_ID);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Product_Product_ID",
                        column: x => x.Product_ID,
                        principalTable: "Product",
                        principalColumn: "Product_ID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product_Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Product_ID = table.Column<int>(type: "int", nullable: true),
                    Customer_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__F1E4639B7F7C9147", x => x.Order_ID);
                    table.ForeignKey(
                        name: "FK__Orders__Customer__7C4F7684",
                        column: x => x.Customer_ID,
                        principalTable: "Customer",
                        principalColumn: "Customer_ID");
                    table.ForeignKey(
                        name: "FK__Orders__Product___7B5B524B",
                        column: x => x.Product_ID,
                        principalTable: "Product",
                        principalColumn: "Product_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Product_ID",
                table: "ActivityLogs",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Customer_ID",
                table: "Event",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Customer_ID",
                table: "Invoice",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Material_Subcontractor_ID",
                table: "Material",
                column: "Subcontractor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Customer_ID",
                table: "Orders",
                column: "Customer_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Product_ID",
                table: "Orders",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Invoice_ID",
                table: "Product",
                column: "Invoice_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Material_ID",
                table: "Product",
                column: "Material_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Subcontractor_ID",
                table: "Product",
                column: "Subcontractor_ID");
            */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Subcontractor");
        }
    }
}
