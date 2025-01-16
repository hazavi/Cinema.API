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
    public class SQLGenreRepository : IGenreRepository
    {
        private readonly MyDbContext dbContext;

        public SQLGenreRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Genre> CreateAsync(Genre genre)
        {
            // Add the genre itself to the database
            await dbContext.Genres.AddAsync(genre);
            await dbContext.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre?> DeleteAsync(int id)
        {
            var existingGenre = await dbContext.Genres.FirstOrDefaultAsync(x => x.GenreId == id);

            if (existingGenre == null)
            {
                return null;
            }

            // Removing the genre will also affect the MovieGenres relationship
            dbContext.Genres.Remove(existingGenre);
            await dbContext.SaveChangesAsync();
            return existingGenre;
        }

        public async Task<List<Genre>> GetAllAsync()
        {
            return await dbContext.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await dbContext.Genres
                         .FirstOrDefaultAsync(x => x.GenreId == id);
        }

        public async Task<Genre?> UpdateAsync(int id, Genre genre)
        {
            var existingGenre = await dbContext.Genres
                                                .Include(g => g.MovieGenres) // Make sure to include the MovieGenres relationship
                                                .ThenInclude(mg => mg.Movie)  // Include related movies if needed
                                                .FirstOrDefaultAsync(x => x.GenreId == id);
            if (existingGenre == null)
            {
                return null;
            }

            // Update the genre name and manage the related movies
            existingGenre.GenreName = genre.GenreName;

            // If necessary, manage the MovieGenres relationship here (e.g., updating associated movies)

            await dbContext.SaveChangesAsync();
            return existingGenre;
        }
    }
}
