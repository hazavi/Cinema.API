using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class MovieandGenresAutoMapper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenresGenreID",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MoviesMovieId",
                table: "MovieGenres");

            migrationBuilder.RenameColumn(
                name: "MoviesMovieId",
                table: "MovieGenres",
                newName: "GenreId");

            migrationBuilder.RenameColumn(
                name: "GenresGenreID",
                table: "MovieGenres",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_MoviesMovieId",
                table: "MovieGenres",
                newName: "IX_MovieGenres_GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId_GenreId",
                table: "MovieGenres",
                columns: new[] { "MovieId", "GenreId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId_GenreId",
                table: "MovieGenres");

            migrationBuilder.RenameColumn(
                name: "GenreId",
                table: "MovieGenres",
                newName: "MoviesMovieId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieGenres",
                newName: "GenresGenreID");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                newName: "IX_MovieGenres_MoviesMovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenresGenreID",
                table: "MovieGenres",
                column: "GenresGenreID",
                principalTable: "Genres",
                principalColumn: "GenreID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MoviesMovieId",
                table: "MovieGenres",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
