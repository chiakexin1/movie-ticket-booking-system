using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingAPI.Data;
using MovieBookingAPI.Models;
using MovieBookingAPI.DTOs;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Genre,
                    m.Duration,
                    m.Description,
                    m.Rating,
                    m.ShowTimes,
                    PosterUrl = string.IsNullOrEmpty(m.PosterUrl) ? null : m.PosterUrl       
                })
                .ToListAsync();

            return Ok(movies);
        }

        // GET: api/movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            return movie;
        }

        // POST: api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie([FromBody] MovieDto movieDto)
        {
            if (movieDto == null)
            {
                return BadRequest("Movie data is required.");
            }

            // ✅ Validation for numeric Duration
            if (movieDto.Duration <= 0 ||
                string.IsNullOrWhiteSpace(movieDto.Title) ||
                string.IsNullOrWhiteSpace(movieDto.Description) ||
                string.IsNullOrWhiteSpace(movieDto.Genre) ||
                string.IsNullOrWhiteSpace(movieDto.PosterUrl) ||
                string.IsNullOrWhiteSpace(movieDto.ShowTimes))
            {
                return BadRequest("All required movie fields must be provided correctly.");
            }


            var movie = new Movie
            {
                Title = movieDto.Title,
                Duration = movieDto.Duration,
                Description = movieDto.Description,
                Genre = movieDto.Genre,
                Rating = movieDto.Rating,
                PosterUrl = movieDto.PosterUrl,
                ShowTimes = movieDto.ShowTimes
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT: api/movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, [FromBody] MovieDto movieDto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            movie.Title = movieDto.Title;
            movie.Duration = movieDto.Duration;
            movie.Description = movieDto.Description;
            movie.Genre = movieDto.Genre;
            movie.Rating = movieDto.Rating;

            // ✅ Only update PosterUrl if provided
            if (!string.IsNullOrWhiteSpace(movieDto.PosterUrl))
            {
                movie.PosterUrl = movieDto.PosterUrl;
            }

            movie.ShowTimes = movieDto.ShowTimes;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }


        // DELETE: api/movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound($"Movie with ID {id} does not exist.");
            }

            try
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("This movie was already deleted or modified by another process.");
            }
        }
    }
}
