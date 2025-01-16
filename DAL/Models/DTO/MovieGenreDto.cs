using DAL.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DAL.Models.DTO
{
    //  (Data Transfer Object) bruges til at overføre data mellem forskellige lag i et program,
    //  typisk mellem lag som database, service og UI, for at begrænse den data, der bliver sendt eller modtaget.
    public class MovieGenreDto
    {
        public int MovieId { get; set; }

        public int GenreId { get; set; }

        [JsonPropertyName("genre_name")]
        public string GenreName { get; set; }
    }
}
