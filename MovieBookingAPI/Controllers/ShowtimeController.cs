using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingAPI.Data;
using MovieBookingAPI.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/venue/{venueId}/showtimes")]
    public class ShowtimeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ShowtimeController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetShowtimes(int venueId)
        {
            var showtimes = await _context.Showtimes
                                          .Where(s => s.VenueId == venueId)
                                          .ToListAsync();
            return Ok(showtimes);
        }
[HttpPost]
public async Task<IActionResult> AddShowtime(int venueId, [FromBody] Showtime newShowtime)
{
    if (newShowtime.EndTime <= newShowtime.StartTime)
        return BadRequest("End time must be after start time.");

    newShowtime.VenueId = venueId;

    var overlap = await _context.Showtimes
        .AnyAsync(s => s.VenueId == venueId &&
                       ((newShowtime.StartTime >= s.StartTime && newShowtime.StartTime < s.EndTime) ||
                        (newShowtime.EndTime > s.StartTime && newShowtime.EndTime <= s.EndTime) ||
                        (newShowtime.StartTime <= s.StartTime && newShowtime.EndTime >= s.EndTime)));

    if (overlap)
        return BadRequest("Showtime overlaps with an existing one.");

    _context.Showtimes.Add(newShowtime);
    await _context.SaveChangesAsync();

    // Return the showtime including movie info
    var showtimeWithMovie = await _context.Showtimes
        .Include(s => s.Movie)
        .FirstOrDefaultAsync(s => s.ShowtimeId == newShowtime.ShowtimeId);

    return CreatedAtAction(nameof(GetShowtimes), new { venueId }, showtimeWithMovie);
}
        [HttpPut("{showtimeId}")]
        public async Task<IActionResult> UpdateShowtime(int venueId, int showtimeId, [FromBody] Showtime updated)
        {
            var showtime = await _context.Showtimes.FirstOrDefaultAsync(s => s.ShowtimeId == showtimeId && s.VenueId == venueId);
            if (showtime == null) return NotFound();

            // Check overlap except itself
            var overlap = await _context.Showtimes
                .AnyAsync(s => s.VenueId == venueId && s.ShowtimeId != showtimeId &&
                               ((updated.StartTime >= s.StartTime && updated.StartTime < s.EndTime) ||
                                 (updated.EndTime > s.StartTime && updated.EndTime <= s.EndTime) ||
                                (updated.StartTime <= s.StartTime && updated.EndTime >= s.EndTime)));

            if (overlap)
                return BadRequest("Updated showtime overlaps with an existing one.");

            showtime.MovieId = updated.MovieId;   
    showtime.StartTime = updated.StartTime;
    showtime.EndTime = updated.EndTime;
            await _context.SaveChangesAsync();
            return Ok(showtime);
        }

        [HttpDelete("{showtimeId}")]
        public async Task<IActionResult> DeleteShowtime(int venueId, int showtimeId)
        {
            var showtime = await _context.Showtimes.FirstOrDefaultAsync(s => s.ShowtimeId == showtimeId && s.VenueId == venueId);
            if (showtime == null) return NotFound();

            _context.Showtimes.Remove(showtime);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}