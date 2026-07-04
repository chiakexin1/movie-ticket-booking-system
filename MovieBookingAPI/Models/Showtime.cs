using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBookingAPI.Models
{
    public class Showtime
    {
        [Key]
        public int ShowtimeId { get; set; }
        
        [Required]
        public string MovieTitle { get; set; } = string.Empty;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
         public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }  

        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; } 
       

    }
}