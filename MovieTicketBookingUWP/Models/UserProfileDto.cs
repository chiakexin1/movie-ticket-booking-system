using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTicketBookingUWP.Models
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; }=string.Empty;
        public string Role { get; set; }
        public string PhoneNumber { get; set; }=string.Empty;
    }
}
