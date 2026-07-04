namespace MovieBookingAPI.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;// e.g. Admin, Customer
    }
}
