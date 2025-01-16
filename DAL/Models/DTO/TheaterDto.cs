using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DTO
{
    //  (Data Transfer Object) bruges til at overføre data mellem forskellige lag i et program,
    //  typisk mellem lag som database, service og UI, for at begrænse den data, der bliver sendt eller modtaget.
    public class TheaterDto
    {
        public int TheaterId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public int AddressId { get; set; }
    }
}
