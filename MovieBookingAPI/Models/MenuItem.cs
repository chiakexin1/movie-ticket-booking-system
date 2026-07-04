namespace MovieBookingAPI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }   // Primary Key

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;
    }
}