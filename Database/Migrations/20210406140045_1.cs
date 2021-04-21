using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Cena = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sklads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sklads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSklads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkladId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSklads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSklads_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSklads_Sklads_SkladId",
                        column: x => x.SkladId,
                        principalTable: "Sklads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dogovors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Cena = table.Column<decimal>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    FIO = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogovors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dogovors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DogovorProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DogovorId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DogovorProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DogovorProducts_Dogovors_DogovorId",
                        column: x => x.DogovorId,
                        principalTable: "Dogovors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DogovorProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DogovorProducts_DogovorId",
                table: "DogovorProducts",
                column: "DogovorId");

            migrationBuilder.CreateIndex(
                name: "IX_DogovorProducts_ProductId",
                table: "DogovorProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogovors_UserId",
                table: "Dogovors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSklads_ProductId",
                table: "ProductSklads",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSklads_SkladId",
                table: "ProductSklads",
                column: "SkladId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DogovorProducts");

            migrationBuilder.DropTable(
                name: "ProductSklads");

            migrationBuilder.DropTable(
                name: "Dogovors");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Sklads");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
