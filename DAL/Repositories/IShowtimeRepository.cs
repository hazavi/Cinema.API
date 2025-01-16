using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // En skabelon, der definerer, hvad en klasse skal kunne.
    public interface IShowtimeRepository
    {
        Task<Showtime> CreateAsync(Showtime showtime);
        Task<List<Showtime>> GetAllAsync();
        Task<Showtime?> GetByIdAsync(int id);
        Task<Showtime?> UpdateAsync(int id, Showtime showtime);
        Task<Showtime?> DeleteAsync(int id);
    }
}
