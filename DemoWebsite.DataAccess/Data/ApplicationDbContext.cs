 using DemoWebsite.Models.Models;
using DemoWebsiteApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoWebsiteApi.DataAccess
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }

        public DbSet<Shipment> shipments { get; set; }

        public DbSet<ShiperAdvice> shiperAdvices { get; set; }

        public DbSet<UserPdf> userPdfs { get; set; }
    }
}
