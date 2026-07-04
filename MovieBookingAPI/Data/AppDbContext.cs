using Microsoft.EntityFrameworkCore;
using MovieBookingAPI.Data; // ✅ Needed for AppDbContext
using MovieBookingAPI.Models; // ✅ Needed for User, Role models


namespace MovieBookingAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<MenuItem>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            
            modelBuilder.Entity<MenuItem>()
                .HasIndex(m => m.Name)
                .IsUnique();

        }
    }
}
