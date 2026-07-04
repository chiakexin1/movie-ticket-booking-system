namespace MovieBookingAPI.Models
{
    public class RegisterUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }= string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleID { get; set; } // foreign key to UserRole
    }
}
