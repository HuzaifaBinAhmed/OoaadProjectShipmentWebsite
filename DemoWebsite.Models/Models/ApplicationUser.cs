using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoWebsiteApi.Models
{
    public class ApplicationUser:IdentityUser
    {

        public string Name { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string Address { get; set; }

        public string PickupAddress { get; set; }

        [NotMapped]
        public string Role { get; set; }






    }
}
