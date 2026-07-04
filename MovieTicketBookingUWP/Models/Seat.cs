using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingUWP.Models
{
    public class Seat
    {
        public int SeatId { get; set; }
        public string SeatCode { get; set; }
        public string Row { get; set; }
        public string SeatType { get; set; }
        public int Number { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int VenueId { get; set; }
    }
}
