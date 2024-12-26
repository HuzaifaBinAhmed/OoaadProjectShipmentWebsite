using System.ComponentModel.DataAnnotations;

namespace DemoWebsiteApi.Models
{
    public class LoginUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
