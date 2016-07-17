using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreCodeCamp.Migrations
{
    public partial class FixFavorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talks_AspNetUsers_CodeCampUserId",
                table: "Talks");

            migrationBuilder.DropIndex(
                name: "IX_Talks_CodeCampUserId",
                table: "Talks");

            migrationBuilder.DropColumn(
                name: "CodeCampUserId",
                table: "Talks");

            migrationBuilder.CreateTable(
                name: "FavoriteTalk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TalkId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteTalk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteTalk_Talks_TalkId",
                        column: x => x.TalkId,
                        principalTable: "Talks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteTalk_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteTalk_TalkId",
                table: "FavoriteTalk",
                column: "TalkId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteTalk_UserId",
                table: "FavoriteTalk",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteTalk");

            migrationBuilder.AddColumn<string>(
                name: "CodeCampUserId",
                table: "Talks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talks_CodeCampUserId",
                table: "Talks",
                column: "CodeCampUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Talks_AspNetUsers_CodeCampUserId",
                table: "Talks",
                column: "CodeCampUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
