using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorBuddy.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ChatGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    StudyPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoteDocuments_StudyPages_StudyPageId",
                        column: x => x.StudyPageId,
                        principalTable: "StudyPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviewImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_NoteDocuments_NoteDocumentId",
                        column: x => x.NoteDocumentId,
                        principalTable: "NoteDocuments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NoteDocumentTag",
                columns: table => new
                {
                    NotesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteDocumentTag", x => new { x.NotesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_NoteDocumentTag_NoteDocuments_NotesId",
                        column: x => x.NotesId,
                        principalTable: "NoteDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteDocumentTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NoteDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StudyPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_ChatGroups_ChatGroupId",
                        column: x => x.ChatGroupId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfile_NoteDocuments_NoteDocumentId",
                        column: x => x.NoteDocumentId,
                        principalTable: "NoteDocuments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfile_StudyPages_StudyPageId",
                        column: x => x.StudyPageId,
                        principalTable: "StudyPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ChatGroupId",
                table: "ChatMessages",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_NoteDocumentId",
                table: "Links",
                column: "NoteDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteDocuments_StudyPageId",
                table: "NoteDocuments",
                column: "StudyPageId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteDocumentTag_TagsId",
                table: "NoteDocumentTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ChatGroupId",
                table: "UserProfile",
                column: "ChatGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_NoteDocumentId",
                table: "UserProfile",
                column: "NoteDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_StudyPageId",
                table: "UserProfile",
                column: "StudyPageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "NoteDocumentTag");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "ChatGroups");

            migrationBuilder.DropTable(
                name: "NoteDocuments");

            migrationBuilder.DropTable(
                name: "StudyPages");
        }
    }
}
