namespace MovieBookingAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public  required int Duration { get; set; }
        public required string Description { get; set; }
        public required string Genre { get; set; }
        public double Rating { get; set; }
        public required string PosterUrl { get; set; }
        public required string ShowTimes { get; set; }
       public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}