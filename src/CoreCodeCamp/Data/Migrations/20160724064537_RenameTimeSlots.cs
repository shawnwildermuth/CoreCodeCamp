using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreCodeCamp.Migrations
{
    public partial class RenameTimeSlots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talks_TalkTimes_TalkTimeId",
                table: "Talks");

            migrationBuilder.DropTable(
                name: "TalkTimes");

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSlots_CodeCampEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "CodeCampEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "RegistrationLink",
                table: "CodeCampEvents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_EventId",
                table: "TimeSlots",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Talks_TimeSlots_TalkTimeId",
                table: "Talks",
                column: "TalkTimeId",
                principalTable: "TimeSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talks_TimeSlots_TalkTimeId",
                table: "Talks");

            migrationBuilder.DropColumn(
                name: "RegistrationLink",
                table: "CodeCampEvents");

            migrationBuilder.DropTable(
                name: "TimeSlots");

            migrationBuilder.CreateTable(
                name: "TalkTimes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TalkTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TalkTimes_CodeCampEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "CodeCampEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TalkTimes_EventId",
                table: "TalkTimes",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Talks_TalkTimes_TalkTimeId",
                table: "Talks",
                column: "TalkTimeId",
                principalTable: "TalkTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
