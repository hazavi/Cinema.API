    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace DAL.Models.DTO
    {
        //  (Data Transfer Object) bruges til at overføre data mellem forskellige lag i et program,
        //  typisk mellem lag som database, service og UI, for at begrænse den data, der bliver sendt eller modtaget.
        public class UserDto
        {
            public int UserId { get; set; }
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string Email { get; set; } = null!;
            public bool IsAdmin { get; set; }
            public DateTime CreateDate { get; set; }
            public int PostalCodeId { get; set; }
        }
    }
