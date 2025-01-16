using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // En skabelon, der definerer, hvad en klasse skal kunne.
    public interface IMovieGenreRepository
    {
        Task<List<MovieGenre>> GetAllAsync();
        Task<List<int>> GetByIdAsync(int movieId);
        Task CreateAsync(int movieId, List<int> genreIds);
        Task DeleteAsync(int movieId, List<int> genreIds);
    }
}
