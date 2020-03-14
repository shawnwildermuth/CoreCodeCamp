using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreCodeCamp.Migrations
{
    public partial class ConverToSessionize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallForSpeakersLink",
                table: "CodeCampEvents");

            migrationBuilder.AddColumn<string>(
                name: "SessionizeEmbedId",
                table: "CodeCampEvents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionizeId",
                table: "CodeCampEvents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionizeEmbedId",
                table: "CodeCampEvents");

            migrationBuilder.DropColumn(
                name: "SessionizeId",
                table: "CodeCampEvents");

            migrationBuilder.AddColumn<string>(
                name: "CallForSpeakersLink",
                table: "CodeCampEvents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
