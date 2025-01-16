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
    public class SQLPostalCodeRepository : IPostalCodeRepository
    {
        private readonly MyDbContext dbContext;

        public SQLPostalCodeRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Retrieve all postal codes
        public async Task<List<PostalCode>> GetAllAsync()
        {
            return await dbContext.PostalCodes.ToListAsync();
        }

        // Retrieve a postal code by its ID
        public async Task<PostalCode?> GetByIdAsync(int id)
        {
            return await dbContext.PostalCodes.FirstOrDefaultAsync(x => x.PostalCodeId == id);
        }

        // Create a new postal code
        public async Task<PostalCode> CreateAsync(PostalCode postalCode)
        {
            await dbContext.PostalCodes.AddAsync(postalCode);
            await dbContext.SaveChangesAsync();
            return postalCode;
        }

        // Delete a postal code by its ID
        public async Task<PostalCode?> DeleteAsync(int id)
        {
            var existingPostalCode = await dbContext.PostalCodes.FirstOrDefaultAsync(x => x.PostalCodeId == id);

            if (existingPostalCode == null)
            {
                return null;
            }

            dbContext.PostalCodes.Remove(existingPostalCode); // No Async remove in EF Core
            await dbContext.SaveChangesAsync();
            return existingPostalCode;
        }

        // Update an existing postal code
        public async Task<PostalCode?> UpdateAsync(int id, PostalCode postalCode)
        {
            var existingPostalCode = await dbContext.PostalCodes.FirstOrDefaultAsync(x => x.PostalCodeId == id);
            if (existingPostalCode == null)
            {
                return null;
            }

            // Update fields
            existingPostalCode.Name = postalCode.Name;
            // Add other fields here as needed

            await dbContext.SaveChangesAsync();
            return existingPostalCode;
        }
    }
}
