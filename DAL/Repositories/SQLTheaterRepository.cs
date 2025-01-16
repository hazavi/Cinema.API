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
    public class SQLTheaterRepository : ITheaterRepository
    {
        private readonly MyDbContext dbContext;

        public SQLTheaterRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Retrieve all theaters
        public async Task<List<Theater>> GetAllAsync()
        {
            return await dbContext.Theaters.Include(t => t.Address).ToListAsync();
        }

        // Retrieve a theater by its ID
        public async Task<Theater?> GetByIdAsync(int id)
        {
            return await dbContext.Theaters.Include(t => t.Address)
                                           .FirstOrDefaultAsync(x => x.TheaterId == id);
        }

        // Create a new theater
        public async Task<Theater> CreateAsync(Theater theater)
        {
            await dbContext.Theaters.AddAsync(theater);
            await dbContext.SaveChangesAsync();
            return theater;
        }

        // Delete a theater by its ID
        public async Task<Theater?> DeleteAsync(int id)
        {
            var existingTheater = await dbContext.Theaters.FirstOrDefaultAsync(x => x.TheaterId == id);

            if (existingTheater == null)
            {
                return null;
            }

            dbContext.Theaters.Remove(existingTheater); // No Async remove in EF Core
            await dbContext.SaveChangesAsync();
            return existingTheater;
        }

        // Update an existing theater
        public async Task<Theater?> UpdateAsync(int id, Theater theater)
        {
            var existingTheater = await dbContext.Theaters.FirstOrDefaultAsync(x => x.TheaterId == id);
            if (existingTheater == null)
            {
                return null;
            }

            // Update fields
            existingTheater.Name = theater.Name;
            existingTheater.Capacity = theater.Capacity;
            existingTheater.Location = theater.Location;
            existingTheater.AddressId = theater.AddressId;

            await dbContext.SaveChangesAsync();
            return existingTheater;
        }
    }
}
