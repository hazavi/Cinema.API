using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain
{
    public class Showtime
    {
        public int ShowtimeId { get; set; }
        public int TheaterId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }


        // Navigation Property
        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
    }
}
