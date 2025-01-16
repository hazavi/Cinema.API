using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // En skabelon, der definerer, hvad en klasse skal kunne.
    public interface ITheaterRepository
    {
        Task<Theater> CreateAsync(Theater theater);
        Task<List<Theater>> GetAllAsync();
        Task<Theater?> GetByIdAsync(int id);
        Task<Theater?> UpdateAsync(int id, Theater theater);
        Task<Theater?> DeleteAsync(int id);
    }
}
