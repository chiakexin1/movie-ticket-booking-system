using System.Collections.Generic;

namespace MovieBookingAPI.Models
{
    public class Venue
    {
    public int VenueId { get; set; }
    public required string Name { get; set; }
    public required string VenueType { get; set; }  // Standard 2D, IMAX, Dolby, 4DX
    public int Capacity { get; set; }
    public List<Seat> Seats { get; set; } = new List<Seat>(); 
    public bool IsActive { get; set; } = true;  
    }
}
