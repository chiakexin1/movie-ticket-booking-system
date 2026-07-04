using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingUWP.Models
{
    public class Showtime
    {
        public int ShowtimeId { get; set; }
        public int VenueId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
