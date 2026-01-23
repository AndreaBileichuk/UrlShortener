using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherPropertiesToShortUrlEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                schema: "App",
                table: "ShortUrls",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "App",
                table: "ShortUrls",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OriginalUrl",
                schema: "App",
                table: "ShortUrls",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                schema: "App",
                table: "ShortUrls",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_CreatedByUserId",
                schema: "App",
                table: "ShortUrls",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortUrls_AspNetUsers_CreatedByUserId",
                schema: "App",
                table: "ShortUrls",
                column: "CreatedByUserId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortUrls_AspNetUsers_CreatedByUserId",
                schema: "App",
                table: "ShortUrls");

            migrationBuilder.DropIndex(
                name: "IX_ShortUrls_CreatedByUserId",
                schema: "App",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                schema: "App",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "App",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "OriginalUrl",
                schema: "App",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                schema: "App",
                table: "ShortUrls");
        }
    }
}
