using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebstoreMyElectronics.Migrations
{
    public partial class confirmusa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authenticateNumber",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "authenticationCode",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authenticationCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "authenticateNumber",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
