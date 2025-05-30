using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace cybersoft_final_project.Entities
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    categoryid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    categoryname = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.categoryid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recommendation_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    slugname = table.Column<string>(name: "slug-name", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fullname = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    roleid = table.Column<int>(type: "int", nullable: true),
                    address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    birthday = table.Column<DateTime>(type: "date", nullable: true),
                    phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.userid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    productid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    productname = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    image = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    importdate = table.Column<DateTime>(type: "date", nullable: true),
                    usingdate = table.Column<DateTime>(type: "date", nullable: true),
                    brand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    sku = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    discount_price = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: true),
                    discount_percent = table.Column<int>(type: "int", nullable: true),
                    stock_quantity = table.Column<int>(type: "int", nullable: true),
                    sold_quantity = table.Column<int>(type: "int", nullable: true),
                    is_available = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    rating_avg = table.Column<float>(type: "float(5,2)", nullable: true),
                    rating_count = table.Column<int>(type: "int", nullable: true),
                    categoryid = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    highlight = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.productid);
                    table.ForeignKey(
                        name: "product_ibfk_1",
                        column: x => x.categoryid,
                        principalTable: "category",
                        principalColumn: "categoryid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    orderid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    orderdate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    total = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: true),
                    userid = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.orderid);
                    table.ForeignKey(
                        name: "order_ibfk_1",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product_image",
                columns: table => new
                {
                    imageid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    productid = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.imageid);
                    table.ForeignKey(
                        name: "product_image_ibfk_1",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "productid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "productattribute",
                columns: table => new
                {
                    attributeid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    productid = table.Column<int>(type: "int", nullable: true),
                    attribute_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    attribute_value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.attributeid);
                    table.ForeignKey(
                        name: "productattribute_ibfk_1",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "productid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "recommendation_product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    recommendation_type_id = table.Column<int>(type: "int", nullable: false),
                    productid = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "recommendation_product_ibfk_1",
                        column: x => x.recommendation_type_id,
                        principalTable: "recommendation_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "recommendation_product_ibfk_2",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "productid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderdetail",
                columns: table => new
                {
                    detailid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    price = table.Column<decimal>(type: "decimal(10,0)", precision: 10, scale: 0, nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    orderid = table.Column<int>(type: "int", nullable: true),
                    productid = table.Column<int>(type: "int", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.detailid);
                    table.ForeignKey(
                        name: "orderdetail_ibfk_1",
                        column: x => x.orderid,
                        principalTable: "order",
                        principalColumn: "orderid");
                    table.ForeignKey(
                        name: "orderdetail_ibfk_2",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "productid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "userid",
                table: "order",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "orderid",
                table: "orderdetail",
                column: "orderid");

            migrationBuilder.CreateIndex(
                name: "productid",
                table: "orderdetail",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "categoryid",
                table: "product",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "productid1",
                table: "product_image",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "productid2",
                table: "productattribute",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "productid3",
                table: "recommendation_product",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "recommendation_type_id",
                table: "recommendation_product",
                column: "recommendation_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderdetail");

            migrationBuilder.DropTable(
                name: "product_image");

            migrationBuilder.DropTable(
                name: "productattribute");

            migrationBuilder.DropTable(
                name: "recommendation_product");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "recommendation_type");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
