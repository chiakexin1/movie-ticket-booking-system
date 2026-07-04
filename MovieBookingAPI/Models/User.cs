using System;

namespace MovieBookingAPI.Models
{

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }= string.Empty;
        public string PasswordHash { get; set; }=   string.Empty;
        public string FullName { get; set; }=   string.Empty;
        public string Email { get; set; }=   string.Empty;
         public string PhoneNumber { get; set; }= string.Empty;
         public int RoleId { get; set; }

        // Navigation property
        public Role Role { get; set; } = null!;
    }
}
