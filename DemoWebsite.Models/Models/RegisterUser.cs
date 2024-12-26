using System.ComponentModel.DataAnnotations;

namespace DemoWebsiteApi.Models
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        public string PhoneNumber { get; set; }

        public string Role { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string PickupAddress { get; set; }

        public string State { get; set; }

    }
}
