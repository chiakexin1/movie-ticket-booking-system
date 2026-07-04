using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingUWP.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public double Rating { get; set; }
        public string PosterFileName { get; set; }
        public string PosterFolder { get; set; }
        public string PosterUrl { get; set; }
        public string ShowTimes { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public ICollection<Showtime> Showtimes { get; set; }
    }
}
