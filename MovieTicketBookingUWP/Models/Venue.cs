using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingUWP.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string Name { get; set; }
        public string VenueType { get; set; }
        public int Capacity { get; set; }

        public List<Seat> Seats { get; set; } = new List<Seat>();
        public bool IsActive { get; set; } = true;
    }
}
