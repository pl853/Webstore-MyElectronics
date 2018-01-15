using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebstoreMyElectronics.Migrations
{
    public partial class reqind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UserSavedProducts_UserSavedProductsId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "UserSavedProducts");

            migrationBuilder.DropIndex(
                name: "IX_Products_UserSavedProductsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserSavedProductsId",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSavedProductsId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSavedProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserSavedProductsId",
                table: "Products",
                column: "UserSavedProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedProducts_UserId",
                table: "UserSavedProducts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UserSavedProducts_UserSavedProductsId",
                table: "Products",
                column: "UserSavedProductsId",
                principalTable: "UserSavedProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
