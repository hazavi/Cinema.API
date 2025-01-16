using DAL.Data;
using DAL.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    //Repository fungerer som en hjælper, der håndterer al kommunikation med databasen
    public class SQLShowtimeRepository : IShowtimeRepository
    {
        private readonly MyDbContext dbContext;

        public SQLShowtimeRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Create a new Showtime
        public async Task<Showtime> CreateAsync(Showtime showtime)
        {
            await dbContext.Showtimes.AddAsync(showtime);
            await dbContext.SaveChangesAsync();
            return showtime;
        }

        // Get all Showtimes
        public async Task<List<Showtime>> GetAllAsync()
        {
            return await dbContext.Showtimes.ToListAsync();
        }

        // Get a Showtime by ID
        public async Task<Showtime?> GetByIdAsync(int id)
        {
            return await dbContext.Showtimes
                         .FirstOrDefaultAsync(x => x.ShowtimeId == id);
        }

        // Update an existing Showtime
        public async Task<Showtime?> UpdateAsync(int id, Showtime showtime)
        {
            var existingShowtime = await dbContext.Showtimes.FirstOrDefaultAsync(x => x.ShowtimeId == id);
            if (existingShowtime == null)
            {
                return null;
            }

            // Update properties
            existingShowtime.MovieId = showtime.MovieId;
            existingShowtime.TheaterId = showtime.TheaterId;
            existingShowtime.StartTime = showtime.StartTime;

            await dbContext.SaveChangesAsync();
            return existingShowtime;
        }

        // Delete a Showtime by ID
        public async Task<Showtime?> DeleteAsync(int id)
        {
            var existingShowtime = await dbContext.Showtimes.FirstOrDefaultAsync(x => x.ShowtimeId == id);

            if (existingShowtime == null)
            {
                return null;
            }

            dbContext.Showtimes.Remove(existingShowtime);
            await dbContext.SaveChangesAsync();
            return existingShowtime;
        }
    }
}
