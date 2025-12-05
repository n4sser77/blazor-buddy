using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorBuddy.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class addedViewedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VisitedAt",
                table: "StudyPages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VisitedAt",
                table: "NoteDocuments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitedAt",
                table: "StudyPages");

            migrationBuilder.DropColumn(
                name: "VisitedAt",
                table: "NoteDocuments");
        }
    }
}
