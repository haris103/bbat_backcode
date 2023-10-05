using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using WEBAPIVersion5.Models;

namespace WEBAPIVersion5.DAL
{
    public class BusinessUnitContext : DbContext
    {
        public BusinessUnitContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BusinessUnit> BusinessUnit { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<Usersystem> Usersystem { get; set; }

        public DbSet<Teamsystem> Teamsystem { get; set; }







    }
}
