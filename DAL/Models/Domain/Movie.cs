using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DAL.Models.Domain
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string PosterUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public decimal Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool isShowing { get; set; }

        [JsonIgnore]
        //gemmer en lister med flere elementer, obj
        // mange-til-mange relation mellem Movie og Genre
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public List<Genre> Genres { get; set; } = new List<Genre>();  // navigation property to Genre entities

    }
}
