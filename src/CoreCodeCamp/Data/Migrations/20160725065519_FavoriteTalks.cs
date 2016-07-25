using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreCodeCamp.Migrations
{
    public partial class FavoriteTalks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteTalk_Talks_TalkId",
                table: "FavoriteTalk");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteTalk_AspNetUsers_UserId",
                table: "FavoriteTalk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteTalk",
                table: "FavoriteTalk");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteTalks",
                table: "FavoriteTalk",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteTalks_Talks_TalkId",
                table: "FavoriteTalk",
                column: "TalkId",
                principalTable: "Talks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteTalks_AspNetUsers_UserId",
                table: "FavoriteTalk",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteTalk_UserId",
                table: "FavoriteTalk",
                newName: "IX_FavoriteTalks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteTalk_TalkId",
                table: "FavoriteTalk",
                newName: "IX_FavoriteTalks_TalkId");

            migrationBuilder.RenameTable(
                name: "FavoriteTalk",
                newName: "FavoriteTalks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteTalks_Talks_TalkId",
                table: "FavoriteTalks");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteTalks_AspNetUsers_UserId",
                table: "FavoriteTalks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteTalks",
                table: "FavoriteTalks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteTalk",
                table: "FavoriteTalks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteTalk_Talks_TalkId",
                table: "FavoriteTalks",
                column: "TalkId",
                principalTable: "Talks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteTalk_AspNetUsers_UserId",
                table: "FavoriteTalks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteTalks_UserId",
                table: "FavoriteTalks",
                newName: "IX_FavoriteTalk_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteTalks_TalkId",
                table: "FavoriteTalks",
                newName: "IX_FavoriteTalk_TalkId");

            migrationBuilder.RenameTable(
                name: "FavoriteTalks",
                newName: "FavoriteTalk");
        }
    }
}
