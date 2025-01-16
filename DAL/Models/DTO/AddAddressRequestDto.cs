using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DTO
{
    //  (Data Transfer Object) bruges til at overføre data mellem forskellige lag i et program,
    //  typisk mellem lag som database, service og UI, for at begrænse den data, der bliver sendt eller modtaget.
    public class AddAddressRequestDto
    {
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public int PostalCodeId { get; set; }
    }
}
