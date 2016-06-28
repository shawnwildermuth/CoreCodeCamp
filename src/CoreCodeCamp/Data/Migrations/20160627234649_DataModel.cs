using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreCodeCamp.Data.Migrations
{
    public partial class DataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Speakers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Bio = table.Column<string>(nullable: true),
                    Blog = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TwitterHandle = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Speakers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TalkTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalkTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Talks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Abstract = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    CodeCampUserId = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    RoomId = table.Column<int>(nullable: true),
                    SpeakerId = table.Column<int>(nullable: true),
                    TalkTimeId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TrackId = table.Column<int>(nullable: true),
                    Votes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talks_AspNetUsers_CodeCampUserId",
                        column: x => x.CodeCampUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talks_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talks_Speakers_SpeakerId",
                        column: x => x.SpeakerId,
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talks_TalkTimes_TalkTimeId",
                        column: x => x.TalkTimeId,
                        principalTable: "TalkTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Talks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    TalkId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Talks_TalkId",
                        column: x => x.TalkId,
                        principalTable: "Talks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TalkId",
                table: "Categories",
                column: "TalkId");

            migrationBuilder.CreateIndex(
                name: "IX_Talks_CodeCampUserId",
                table: "Talks",
                column: "CodeCampUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Talks_RoomId",
                table: "Talks",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Talks_SpeakerId",
                table: "Talks",
                column: "SpeakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Talks_TalkTimeId",
                table: "Talks",
                column: "TalkTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Talks_TrackId",
                table: "Talks",
                column: "TrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Talks");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Speakers");

            migrationBuilder.DropTable(
                name: "TalkTimes");

            migrationBuilder.DropTable(
                name: "Tracks");
        }
    }
}
