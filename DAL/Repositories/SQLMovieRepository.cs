using DAL.Data;
using DAL.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    //Repository fungerer som en hjælper, der håndterer al kommunikation med databasen
    public class SQLMovieRepository : IMovieRepository
    {
        private readonly MyDbContext dbContext;

        public SQLMovieRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            // Extract and validate genre IDs
            var genreIds = movie.MovieGenres.Select(mg => mg.GenreId).Distinct().ToList();
            var existingGenres = await dbContext.Genres
                                                 .Where(g => genreIds.Contains(g.GenreId))
                                                 .ToListAsync();

            // Check if all genres are valid
            if (existingGenres.Count != genreIds.Count)
            {
                throw new ArgumentException("One or more genres are invalid.");
            }

            // Create the MovieGenre associations with genres
            movie.MovieGenres = existingGenres.Select(genre => new MovieGenre
            {
                GenreId = genre.GenreId,
                Genre = genre,
                Movie = movie // Add the movie reference
            }).ToList();

            // Add the movie to the database
            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();  // This saves the movie and generates movieId

            // Ensure the movieId is now set
            if (movie.MovieId == 0)
            {
                throw new InvalidOperationException("MovieId is still 0 after SaveChangesAsync.");
            }

            // Now that we have a valid movieId, update the MovieGenres with the correct movieId
            foreach (var movieGenre in movie.MovieGenres)
            {
                movieGenre.MovieId = movie.MovieId;  // Ensure MovieId is assigned correctly
            }

            // Log the genres with the updated movieId
            foreach (var genre in movie.MovieGenres)
            {
                Console.WriteLine($"GenreId: {genre.GenreId}, MovieId: {genre.MovieId}");
            }

            // Update the MovieGenres table
            dbContext.MovieGenres.UpdateRange(movie.MovieGenres);
            await dbContext.SaveChangesAsync();

            // Return the movie with the correct movieId
            return movie;
        }



        public async Task<List<Movie>> GetAllAsync()
        {
            // Include related genres in the query to get the full data
            return await dbContext.Movies
                                   .Include(m => m.MovieGenres)
                                   .ThenInclude(mg => mg.Genre)
                                   .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            // Include related genres when fetching a single movie
            return await dbContext.Movies
                                   .Include(m => m.MovieGenres)
                                   .ThenInclude(mg => mg.Genre)
                                   .FirstOrDefaultAsync(x => x.MovieId == id);
        }

        public async Task<Movie?> UpdateAsync(int id, Movie movie)
        {
            var existingMovie = await dbContext.Movies
                                               .Include(m => m.MovieGenres)
                                               .ThenInclude(mg => mg.Genre)
                                               .FirstOrDefaultAsync(x => x.MovieId == id);
            if (existingMovie == null)
            {
                return null;
            }

            existingMovie.PosterUrl = movie.PosterUrl;
            existingMovie.Title = movie.Title;
            existingMovie.Description = movie.Description;
            existingMovie.DurationMinutes = movie.DurationMinutes;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.Rating = movie.Rating;
            existingMovie.isShowing = movie.isShowing;


            // Remove old genres
            dbContext.MovieGenres.RemoveRange(existingMovie.MovieGenres);

            // Add new genres
            var genreIds = movie.MovieGenres.Select(mg => mg.GenreId).Distinct().ToList();
            var existingGenres = await dbContext.Genres.Where(g => genreIds.Contains(g.GenreId)).ToListAsync();

            existingMovie.MovieGenres = existingGenres.Select(genre => new MovieGenre
            {
                GenreId = genre.GenreId,
                Movie = existingMovie,
                Genre = genre
            }).ToList();

            await dbContext.SaveChangesAsync();
            return existingMovie;
        }

        public async Task<Movie?> DeleteAsync(int id)
        {
            var existingMovie = await dbContext.Movies
                                               .Include(m => m.MovieGenres)
                                               .ThenInclude(mg => mg.Genre)
                                               .FirstOrDefaultAsync(x => x.MovieId == id);

            if (existingMovie == null)
            {
                return null;
            }

            // Remove related MovieGenres first to prevent foreign key issues
            dbContext.MovieGenres.RemoveRange(existingMovie.MovieGenres);
            dbContext.Movies.Remove(existingMovie); // There is no Async remove in EF at this time.
            await dbContext.SaveChangesAsync();
            return existingMovie;
        }

    }
}
