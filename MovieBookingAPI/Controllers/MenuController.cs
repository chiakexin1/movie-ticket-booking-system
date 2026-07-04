using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingAPI.Data;
using MovieBookingAPI.Models;

namespace MovieBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<MenuController> _logger;

        public MenuController(AppDbContext context, IWebHostEnvironment environment, ILogger<MenuController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> Get()
        {
            try
            {
                _logger.LogInformation("Getting all menu items");
                return await _context.MenuItems.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> Post(MenuItem item)
        {
            try
            {
                _logger.LogInformation($"Adding new menu item: {item.Name}");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state");
                    return BadRequest(ModelState);
                }

                _context.MenuItems.Add(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Item added with ID: {item.Id}");
                return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while adding menu item");
                return StatusCode(500, "Database error occurred");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding menu item");
                return StatusCode(500, "Internal server error");
            }
        }

        // ✅ Add similar try-catch to all other methods
        // DELETE: api/menu/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> Delete(int id)
        {
            var job = await _context.MenuItems.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.MenuItems.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
        
        // PUT: api/menu/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(int id, MenuItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



}
}
