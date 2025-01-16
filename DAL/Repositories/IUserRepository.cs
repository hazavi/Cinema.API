using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // En skabelon, der definerer, hvad en klasse skal kunne.
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user, string password);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> UpdateAsync(int id, User user, string? newPassword);
        Task<User?> DeleteAsync(int id);
        Task<User?> GetByEmailAsync(string email);

        Task<User?> SetIsAdminAsync(int id, bool isAdmin);
    }
}
