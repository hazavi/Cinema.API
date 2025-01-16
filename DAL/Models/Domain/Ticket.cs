using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Domain
{
    public class Ticket
    {
        public int TicketId { get; set; } 
        public int UserId { get; set; }   
        public int ShowtimeId { get; set; } 
        public int SeatId { get; set; }   
        public DateTime PurchaseDate { get; set; }  
        public int Price { get; set; }
        public int Quantity { get; set; }

        // Navigation properties 
        public User User { get; set; }       
        public Showtime Showtime { get; set; }  
        public Seat Seat { get; set; }
    }

}
