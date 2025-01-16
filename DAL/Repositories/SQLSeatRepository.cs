using DAL.Data;
using DAL.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    //Repository fungerer som en hjælper, der håndterer al kommunikation med databasen
    public class SQLSeatRepository : ISeatRepository
    {
        private readonly MyDbContext dbContext;

        public SQLSeatRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get all Seats
        public async Task<List<Seat>> GetAllAsync()
        {
            return await dbContext.Seats.ToListAsync();
        }

        // Get a Seat by ID
        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await dbContext.Seats
                         .FirstOrDefaultAsync(x => x.SeatId == id);
        }

        // Create a new Seat
        public async Task<Seat> CreateAsync(Seat seat)
        {
            await dbContext.Seats.AddAsync(seat);
            await dbContext.SaveChangesAsync();
            return seat;
        }

        // Delete a Seat by ID
        public async Task<Seat?> DeleteAsync(int id)
        {
            var existingSeat = await dbContext.Seats.FirstOrDefaultAsync(x => x.SeatId == id);

            if (existingSeat == null)
            {
                return null;
            }

            dbContext.Seats.Remove(existingSeat);
            await dbContext.SaveChangesAsync();
            return existingSeat;
        }

        // Update an existing Seat
        public async Task<Seat?> UpdateAsync(int id, Seat seat)
        {
            var existingSeat = await dbContext.Seats.FirstOrDefaultAsync(x => x.SeatId == id);
            if (existingSeat == null)
            {
                return null;
            }

            // Update properties
            existingSeat.RowNumber = seat.RowNumber;
            existingSeat.SeatNumber = seat.SeatNumber;
            existingSeat.IsAvailable = seat.IsAvailable;

            await dbContext.SaveChangesAsync();
            return existingSeat;
        }
    }
}
