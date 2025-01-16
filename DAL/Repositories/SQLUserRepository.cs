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
    // Repository fungerer som en hjælper, der håndterer al kommunikation med databasen

    // Klasse arvet User interfacet og klassen skal implementere alle funktioner der er defineret i interfacet
    public class SQLUserRepository : IUserRepository  
    {
        private readonly MyDbContext dbContext;

        // En konstruktor bruges til at initialisere klassen, når den bliver oprettet.
        public SQLUserRepository(MyDbContext dbContext) // tager parameter dbContext som repræsenterer databaseforbindelse.
        {
            this.dbContext = dbContext; // gemmer forbindelse i variablen, som bruges i koden 
        }

        // CREATE | Opretter en ny bruger i db med en hash og salt til password

        // async: Fortæller, at funktionen arbejder asynkront. Den kan pause arbejdet (vente) uden at blokere resten af programmet.
        // Task : Funktionen returnerer en opgave (task), som til sidst giver en User
        public async Task<User> CreateAsync(User user, string password) // Constructor med 2 parameter
        {
            // Generere Hash and Salt til password
            // Destructuring - funktion returnerer flere værdier som en tuple (en samling af værdier),
            // og vi kan gemme dem direkte i separate variabler.
            var (passwordHash, passwordSalt) = CreatePasswordHash(password); 

            // Assigner eller gemmes hash and salt til user
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // Gemmer user til db
            // await bruges med async, Det fortæller programmet, at det skal vente på, at en asynkron opgave bliver færdig,
            // før det fortsætter til den næste linje.
            await dbContext.Users.AddAsync(user); // tilføjer brugeren til databasen.
            await dbContext.SaveChangesAsync(); // Gemmer ændringerne i databasen

            return user; // Returnerer oprettede user, når alt er færdigt.
        }

        // DELETE user by Id | 
        public async Task<User?> DeleteAsync(int id) 
        {
            // Find brugeren baseret på ID
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id); // lambda expression

            // Returner null, hvis brugeren ikke findes
            if (existingUser == null)
            {
                return null;
            }

            dbContext.Users.Remove(existingUser); // Fjern brugeren fra databasen
            await dbContext.SaveChangesAsync(); // Gem ændringerne i databasen
            return existingUser; // Returner den slettede bruger
        }

        // GET all users
        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();  // Returner alle brugere som en liste
        }

        // GET user by Id
        public async Task<User?> GetByIdAsync(int id)
        {
            // Find og returner brugeren baseret på ID
            return await dbContext.Users
                         .FirstOrDefaultAsync(x => x.UserId == id);
        }

        // UPDATE | Opdater en brugers oplysninger
        public async Task<User?> UpdateAsync(int id, User user, string? newPassword = null)
        {
            // Find brugeren baseret på ID
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            // Returner null, hvis brugeren ikke findes
            if (existingUser == null)
            {
                return null;
            }

            // Opdater brugerens oplysninger
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;

            // Hvis en ny adgangskode er angivet, opdater hash og salt
            if (!string.IsNullOrEmpty(newPassword))
            {
                var (passwordHash, passwordSalt) = CreatePasswordHash(newPassword);
                existingUser.PasswordHash = passwordHash;
                existingUser.PasswordSalt = passwordSalt;
            }

            await dbContext.SaveChangesAsync(); // Gem ændringerne i db
            return existingUser; // Returner den opdaterede user
        }


        // HELPER | Opret hash og salt til en adgangskode
        private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
        {
            // Bruger HMACSHA512 til at generere en sikker hash og salt
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            var passwordSalt = hmac.Key; // HMAC's key is the salt
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Hash the password
            return (passwordHash, passwordSalt);
        }

        // GET BY EMAIL | Find en bruger baseret på email
        public async Task<User?> GetByEmailAsync(string email)
        {
            // Returnerer den første bruger, der matcher den angivne e-mail
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        // SET IS ADMIN | Opdaterer om en bruger er administrator
        public async Task<User?> SetIsAdminAsync(int id, bool isAdmin)
        {
            // Find brugeren baseret på ID
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            // Returner null, hvis brugeren ikke findes
            if (user == null)
            {
                return null;
            }

            user.IsAdmin = isAdmin; // Opdater administratorstatus
            await dbContext.SaveChangesAsync(); // Gem ændringerne i db
            return user; // Returner den opdaterede user
        }


    }
}
