using Microsoft.EntityFrameworkCore;
using WEBAPIVersion5.Models;

namespace WEBAPIVersion5.DAL
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Userdata> UserData { get; set; }
    }
}
