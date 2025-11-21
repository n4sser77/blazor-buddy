using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorBuddy.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserModelRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_ChatGroups_ChatGroupId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_ChatGroupId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "ChatGroupId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "StudyPages");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "NoteDocuments");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "StudyPages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "NoteDocuments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FromUserId",
                table: "ChatMessages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ChatGroupUserProfile",
                columns: table => new
                {
                    ChatGroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroupUserProfile", x => new { x.ChatGroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatGroupUserProfile_ChatGroups_ChatGroupsId",
                        column: x => x.ChatGroupsId,
                        principalTable: "ChatGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatGroupUserProfile_UserProfile_UsersId",
                        column: x => x.UsersId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudyPages_OwnerId",
                table: "StudyPages",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteDocuments_OwnerId",
                table: "NoteDocuments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_FromUserId",
                table: "ChatMessages",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroupUserProfile_UsersId",
                table: "ChatGroupUserProfile",
                column: "UsersId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_UserProfile_FromUserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteDocuments_UserProfile_OwnerId",
                table: "NoteDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyPages_UserProfile_OwnerId",
                table: "StudyPages");

            migrationBuilder.DropTable(
                name: "ChatGroupUserProfile");

            migrationBuilder.DropIndex(
                name: "IX_StudyPages_OwnerId",
                table: "StudyPages");

            migrationBuilder.DropIndex(
                name: "IX_NoteDocuments_OwnerId",
                table: "NoteDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_FromUserId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "StudyPages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "NoteDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatGroupId",
                table: "UserProfile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "StudyPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "NoteDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "FromUserId",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ChatGroupId",
                table: "UserProfile",
                column: "ChatGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_ChatGroups_ChatGroupId",
                table: "UserProfile",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "Id");
        }
    }
}
