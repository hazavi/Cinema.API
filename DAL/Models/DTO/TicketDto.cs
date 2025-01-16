using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DTO
{
    //  (Data Transfer Object) bruges til at overføre data mellem forskellige lag i et program,
    //  typisk mellem lag som database, service og UI, for at begrænse den data, der bliver sendt eller modtaget.
    public class TicketDto
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
