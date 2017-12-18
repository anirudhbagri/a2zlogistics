using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<FinalProject.Models.AspNetUser> AspNetUsers { get; set; }

        public System.Data.Entity.DbSet<FinalProject.Models.Transaction> Transactions { get; set; }
    }
}