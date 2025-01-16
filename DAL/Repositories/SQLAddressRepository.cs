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
    public class SQLAddressRepository : IAddressRepository
    {
        private readonly MyDbContext dbContext;

        public SQLAddressRepository(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Address>> GetAllAsync()
        {
            return await dbContext.Addresses.ToListAsync();
        }
        public async Task<Address?> GetByIdAsync(int id)
        {
            return await dbContext.Addresses
                         .FirstOrDefaultAsync(x => x.AddressId == id);

        }
        public async Task<Address> CreateAsync(Address address)
        {
            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<Address?> DeleteAsync(int id)
        {
            var existingAddress = await dbContext.Addresses.FirstOrDefaultAsync(x => x.AddressId == id);

            if (existingAddress == null)
            {
                return null;
            }

            dbContext.Addresses.Remove(existingAddress); // There is no Async remove in EF at this time.
            await dbContext.SaveChangesAsync();
            return existingAddress;
        }



        public async Task<Address?> UpdateAsync(int id, Address address)
        {
            var existingAddress = await dbContext.Addresses.FirstOrDefaultAsync(x => x.AddressId == id);
            if (existingAddress == null)
            {
                return null;
            }

            existingAddress.StreetName = address.StreetName;
            existingAddress.StreetNumber = address.StreetNumber;

            await dbContext.SaveChangesAsync();
            return existingAddress;
        }
    }
}
