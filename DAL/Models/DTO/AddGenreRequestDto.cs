using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DTO
{
    //  (Data Transfer Object) bruges til at overføre data mellem forskellige lag i et program,
    //  typisk mellem lag som database, service og UI, for at begrænse den data, der bliver sendt eller modtaget.
    public class AddGenreRequestDto
    {
        public string GenreName { get; set; }

        // mange-til-mange relation, 
        // en genre kan være knyttet til flere film, og en film kan have flere genrer.
        public List<MovieDto> Movie { get; set; } = new List<MovieDto>();
    }
}
