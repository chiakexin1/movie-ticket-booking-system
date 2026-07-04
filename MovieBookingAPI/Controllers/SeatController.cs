using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingAPI.Data;
using MovieBookingAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/venue/{venueId}/seats")]
    public class SeatController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SeatController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetSeats(int venueId)
        {
            var venue = await _context.Venues
                                      .Include(v => v.Seats)
                                      .FirstOrDefaultAsync(v => v.VenueId == venueId);
            if (venue == null) return NotFound("Venue not found");

            return Ok(venue.Seats);
        }

        [HttpPost]
        public async Task<IActionResult> AddSeat(int venueId, [FromBody] Seat newSeat)
        {
            var venue = await _context.Venues
                                      .Include(v => v.Seats)
                                      .FirstOrDefaultAsync(v => v.VenueId == venueId);
            if (venue == null) return NotFound("Venue not found");

            newSeat.SeatCode = $"{newSeat.Row}{newSeat.Number}";
            newSeat.VenueId = venueId;

            if (venue.Seats.Any(s => s.SeatCode == newSeat.SeatCode))
                return BadRequest("Seat already exists");

            _context.Seats.Add(newSeat);
            await _context.SaveChangesAsync();
            return Ok(newSeat);
        }

        [HttpPut("{seatId:int}")]
        public async Task<IActionResult> UpdateSeat(int venueId, int seatId, [FromBody] Seat updatedSeat)
        {
            var seat = await _context.Seats
                                     .FirstOrDefaultAsync(s => s.VenueId == venueId && s.SeatId == seatId);
            if (seat == null) return NotFound("Seat not found");

            seat.SeatType = updatedSeat.SeatType;
            seat.AdultPrice = updatedSeat.AdultPrice;
            seat.ChildPrice = updatedSeat.ChildPrice;
            seat.IsAvailable = updatedSeat.IsAvailable;

            await _context.SaveChangesAsync();
            return Ok(seat);
        }

        [HttpDelete("{seatId:int}")]
        public async Task<IActionResult> DeleteSeat(int venueId, int seatId)
        {
            var seat = await _context.Seats
                                     .FirstOrDefaultAsync(s => s.VenueId == venueId && s.SeatId == seatId);
            if (seat == null) return NotFound("Seat not found");

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}