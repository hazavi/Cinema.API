using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public DateTime CreateDate { get; set; }
        public int PostalCodeId { get; set; }


        // Navigation Property
        public PostalCode PostalCode { get; set; }
    }
}
