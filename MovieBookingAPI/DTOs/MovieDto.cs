namespace MovieBookingAPI.DTOs
{

    public class MovieDto
    {
        public required string Title { get; set; }
        public  int Duration { get; set; }
        public required string Description { get; set; }
        public required string Genre { get; set; }
        public double Rating { get; set; }
        public required string PosterUrl { get; set; }
        public required string ShowTimes { get; set; }
    }
}