using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        //gemmer en lister med flere elementer, obj
        // mange-til-mange relation mellem Movie og Genre
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
