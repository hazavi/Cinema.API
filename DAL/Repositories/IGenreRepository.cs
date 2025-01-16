using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // En skabelon, der definerer, hvad en klasse skal kunne.
    public interface IGenreRepository
    {
        Task<Genre> CreateAsync(Genre genre);
        Task<List<Genre>> GetAllAsync();
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre?> UpdateAsync(int id, Genre genre);
        Task<Genre?> DeleteAsync(int id);
    }
}
