using DAL.Data;
using DAL.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    //Repository fungerer som en hjælper, der håndterer al kommunikation med databasen
    public class SQLMovieGenreRepository : IMovieGenreRepository
    {
        private readonly MyDbContext dbContext;

        public SQLMovieGenreRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(int movieId, List<int> genreIds)
        {
            // Check if the movie exists
            var movieExists = await dbContext.Movies.AnyAsync(m => m.MovieId == movieId);
            if (!movieExists)
                throw new KeyNotFoundException("Movie not found.");

            // Fetch existing associations to avoid duplicates
            var existingGenreIds = await dbContext.MovieGenres
                .Where(mg => mg.MovieId == movieId)
                .Select(mg => mg.GenreId)
                .ToListAsync();

            // Find genres to add
            var newGenreIds = genreIds.Except(existingGenreIds).ToList();

            if (newGenreIds.Any())
            {
                // Prepare new associations
                var newAssociations = newGenreIds.Select(genreId => new MovieGenre
                {
                    MovieId = movieId,
                    GenreId = genreId
                }).ToList();

                // Add new associations and save changes
                await dbContext.MovieGenres.AddRangeAsync(newAssociations);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int movieId, List<int> genreIds)
        {
            var movieGenresToDelete = await dbContext.MovieGenres
                .Where(mg => mg.MovieId == movieId && genreIds.Contains(mg.GenreId))
                .ToListAsync();

            if (!movieGenresToDelete.Any())
                throw new InvalidOperationException("No associations found to delete.");

            dbContext.MovieGenres.RemoveRange(movieGenresToDelete); // Remove the specified associations
            await dbContext.SaveChangesAsync(); // Commit the changes to the database
        }

        public async Task<List<MovieGenre>> GetAllAsync()
        {
            return await dbContext.MovieGenres
                .Include(mg => mg.Movie)
                .Include(mg => mg.Genre)
                .ToListAsync(); // Retrieve all associations with related Movie and Genre
        }

        public async Task<List<int>> GetByIdAsync(int movieId)
        {
            return await dbContext.MovieGenres
                .Where(mg => mg.MovieId == movieId)
                .Select(mg => mg.GenreId)
                .ToListAsync(); // Return a list of genre IDs for the given movie
        }
    }
}
