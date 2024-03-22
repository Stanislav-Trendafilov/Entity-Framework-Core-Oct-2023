using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P02_FootballBetting.Data.Migrations
{
    public partial class PlayerTeamNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TownId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Players_TownId",
                table: "Players",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Towns_TownId",
                table: "Players",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "TownId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Towns_TownId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_TownId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Players");
        }
    }
}
