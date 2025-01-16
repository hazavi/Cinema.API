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
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string PosterUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool isShowing { get; set; }

        // MovieGenres: Samling af genre-informationer, der viser, hvilke genrer filmen er knyttet til.
        // Dette er en én-til-mange relation, hvor én film kan have flere genrer.
        public ICollection<MovieGenreDto> MovieGenres { get; set; } = new List<MovieGenreDto>();

        // Dette gør det nemt at hente og arbejde med genre-data for filmen i andre lag af applikationen.
        public List<int> GenreIds { get; set; } = new List<int>();

    }
}
