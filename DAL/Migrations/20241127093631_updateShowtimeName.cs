using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateShowtimeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_showtimes_Movies_MovieId",
                table: "showtimes");

            migrationBuilder.DropForeignKey(
                name: "FK_showtimes_Theaters_TheaterId",
                table: "showtimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_showtimes",
                table: "showtimes");

            migrationBuilder.RenameTable(
                name: "showtimes",
                newName: "Showtimes");

            migrationBuilder.RenameIndex(
                name: "IX_showtimes_TheaterId",
                table: "Showtimes",
                newName: "IX_Showtimes_TheaterId");

            migrationBuilder.RenameIndex(
                name: "IX_showtimes_MovieId",
                table: "Showtimes",
                newName: "IX_Showtimes_MovieId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Showtimes",
                table: "Showtimes",
                column: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Movies_MovieId",
                table: "Showtimes",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Theaters_TheaterId",
                table: "Showtimes",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "TheaterId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Movies_MovieId",
                table: "Showtimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Theaters_TheaterId",
                table: "Showtimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Showtimes",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Seats");

            migrationBuilder.RenameTable(
                name: "Showtimes",
                newName: "showtimes");

            migrationBuilder.RenameIndex(
                name: "IX_Showtimes_TheaterId",
                table: "showtimes",
                newName: "IX_showtimes_TheaterId");

            migrationBuilder.RenameIndex(
                name: "IX_Showtimes_MovieId",
                table: "showtimes",
                newName: "IX_showtimes_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_showtimes",
                table: "showtimes",
                column: "ShowtimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_showtimes_Movies_MovieId",
                table: "showtimes",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_showtimes_Theaters_TheaterId",
                table: "showtimes",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "TheaterId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
