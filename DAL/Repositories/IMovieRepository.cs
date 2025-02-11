using DAL.Models.Domain;
using DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // En skabelon, der definerer, hvad en klasse skal kunne.
    public interface IMovieRepository
    {
        Task<Movie> CreateAsync(Movie movie);
        Task<List<MovieDto>> GetAllAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie?> UpdateAsync(int id, Movie movie);
        Task<Movie?> DeleteAsync(int id);
    }
}
