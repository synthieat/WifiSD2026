using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SD.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediumTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediumTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MediumTypeCode = table.Column<string>(type: "nvarchar(8)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movies_MediumTypes_MediumTypeCode",
                        column: x => x.MediumTypeCode,
                        principalTable: "MediumTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Comedy" },
                    { 3, "Drama" },
                    { 4, "Horror" },
                    { 5, "Science Fiction" }
                });

            migrationBuilder.InsertData(
                table: "MediumTypes",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "BD", "Blu-ray Disc" },
                    { "CD", "Compact Disc" },
                    { "DVD", "Digital Versatile Disc" },
                    { "STREAM", "Streaming" },
                    { "VHS", "Video Home System" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "GenreId", "MediumTypeCode", "Price", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 1, "DVD", 9.99m, new DateTime(1988, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Die Hard" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 1, "BD", 14.99m, new DateTime(2015, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mad Max: Fury Road" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 2, "DVD", 7.99m, new DateTime(1993, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Groundhog Day" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 2, "BD", 12.99m, new DateTime(2014, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Grand Budapest Hotel" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), 3, "DVD", 8.99m, new DateTime(1994, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Shawshank Redemption" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), 3, "BD", 11.99m, new DateTime(1994, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forrest Gump" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), 4, "DVD", 9.99m, new DateTime(1980, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Shining" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), 4, "STREAM", 4.99m, new DateTime(2018, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "A Quiet Place" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), 5, "BD", 13.99m, new DateTime(1982, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Blade Runner" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), 5, "DVD", 10.99m, new DateTime(1999, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Matrix" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GenreId",
                table: "Movies",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MediumTypeCode",
                table: "Movies",
                column: "MediumTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "MediumTypes");
        }
    }
}
