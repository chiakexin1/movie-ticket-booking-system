using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBookingAPI.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; } 
       public required string SeatCode { get; set; }
        [Required]
        public required string Row { get; set; }

        [Required]
        public required string SeatType { get; set; }

        public int Number { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public bool IsAvailable { get; set; }

        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; } 
    }
}