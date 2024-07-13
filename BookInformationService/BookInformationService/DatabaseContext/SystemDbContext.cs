using BookInformationService.Models;
using Microsoft.EntityFrameworkCore;


/* Ensure Migration and Database Initialization:

Open Package Manager Console:
Navigate to Tools > NuGet Package Manager > Package Manager Console in Visual Studio.

Execute the following command:
    Add-Migration InitialCreate
    Update-Database

*/

namespace BookInformationService.DatabaseContext
{
    public class SystemDbContext : DbContext
    {
        public DbSet<BookInformation> BookInformations { get; set; }

        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Ignore<ReservedBookInfo>();
            //modelBuilder.Ignore<ReservationHistory>();
        }
    }
}
