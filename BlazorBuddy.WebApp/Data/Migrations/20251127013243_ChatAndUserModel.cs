using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorBuddy.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class ChatAndUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroupUserProfile_UserProfile_UsersId",
                table: "ChatGroupUserProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_UserProfile_FromUserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteDocuments_UserProfile_OwnerId",
                table: "NoteDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyPages_UserProfile_OwnerId",
                table: "StudyPages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_NoteDocuments_NoteDocumentId",
                table: "UserProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_StudyPages_StudyPageId",
                table: "UserProfile");

            migrationBuilder.DropTable(
                name: "AspNetUserPasskeys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile");

            migrationBuilder.RenameTable(
                name: "UserProfile",
                newName: "UserProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfile_StudyPageId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_StudyPageId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfile_NoteDocumentId",
                table: "UserProfiles",
                newName: "IX_UserProfiles_NoteDocumentId");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "StudyPages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<Guid>(
                name: "CanvasId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Canvases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanvasData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canvases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Canvases_UserProfiles_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NoteDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_NoteDocuments_NoteDocumentId",
                        column: x => x.NoteDocumentId,
                        principalTable: "NoteDocuments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Images_UserProfiles_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CanvasNoteDocument",
                columns: table => new
                {
                    CanvasesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteDocumentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanvasNoteDocument", x => new { x.CanvasesId, x.NoteDocumentsId });
                    table.ForeignKey(
                        name: "FK_CanvasNoteDocument_Canvases_CanvasesId",
                        column: x => x.CanvasesId,
                        principalTable: "Canvases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanvasNoteDocument_NoteDocuments_NoteDocumentsId",
                        column: x => x.NoteDocumentsId,
                        principalTable: "NoteDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CanvasId",
                table: "UserProfiles",
                column: "CanvasId");

            migrationBuilder.CreateIndex(
                name: "IX_Canvases_OwnerId",
                table: "Canvases",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CanvasNoteDocument_NoteDocumentsId",
                table: "CanvasNoteDocument",
                column: "NoteDocumentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_NoteDocumentId",
                table: "Images",
                column: "NoteDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_OwnerId",
                table: "Images",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroupUserProfile_UserProfiles_UsersId",
                table: "ChatGroupUserProfile",
                column: "UsersId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_UserProfiles_FromUserId",
                table: "ChatMessages",
                column: "FromUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteDocuments_UserProfiles_OwnerId",
                table: "NoteDocuments",
                column: "OwnerId",
                principalTable: "UserProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyPages_UserProfiles_OwnerId",
                table: "StudyPages",
                column: "OwnerId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_Id",
                table: "UserProfiles",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Canvases_CanvasId",
                table: "UserProfiles",
                column: "CanvasId",
                principalTable: "Canvases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_NoteDocuments_NoteDocumentId",
                table: "UserProfiles",
                column: "NoteDocumentId",
                principalTable: "NoteDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_StudyPages_StudyPageId",
                table: "UserProfiles",
                column: "StudyPageId",
                principalTable: "StudyPages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroupUserProfile_UserProfiles_UsersId",
                table: "ChatGroupUserProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_UserProfiles_FromUserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteDocuments_UserProfiles_OwnerId",
                table: "NoteDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyPages_UserProfiles_OwnerId",
                table: "StudyPages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_Id",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Canvases_CanvasId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_NoteDocuments_NoteDocumentId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_StudyPages_StudyPageId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "CanvasNoteDocument");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Canvases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_CanvasId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CanvasId",
                table: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfile");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_StudyPageId",
                table: "UserProfile",
                newName: "IX_UserProfile_StudyPageId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_NoteDocumentId",
                table: "UserProfile",
                newName: "IX_UserProfile_NoteDocumentId");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "StudyPages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfile",
                table: "UserProfile",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetUserPasskeys",
                columns: table => new
                {
                    CredentialId = table.Column<byte[]>(type: "varbinary(1024)", maxLength: 1024, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserPasskeys", x => x.CredentialId);
                    table.ForeignKey(
                        name: "FK_AspNetUserPasskeys_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserPasskeys_UserId",
                table: "AspNetUserPasskeys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroupUserProfile_UserProfile_UsersId",
                table: "ChatGroupUserProfile",
                column: "UsersId",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_UserProfile_FromUserId",
                table: "ChatMessages",
                column: "FromUserId",
                principalTable: "UserProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteDocuments_UserProfile_OwnerId",
                table: "NoteDocuments",
                column: "OwnerId",
                principalTable: "UserProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyPages_UserProfile_OwnerId",
                table: "StudyPages",
                column: "OwnerId",
                principalTable: "UserProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_NoteDocuments_NoteDocumentId",
                table: "UserProfile",
                column: "NoteDocumentId",
                principalTable: "NoteDocuments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_StudyPages_StudyPageId",
                table: "UserProfile",
                column: "StudyPageId",
                principalTable: "StudyPages",
                principalColumn: "Id");
        }
    }
}
