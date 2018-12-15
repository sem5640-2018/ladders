using Microsoft.EntityFrameworkCore;

namespace ladders.Models
{
    public class LaddersContext : DbContext
    {
        public LaddersContext (DbContextOptions<LaddersContext> options)
            : base(options)
        {
        }

        public DbSet<ProfileModel> ProfileModel { get; set; }

        public DbSet<LadderModel> LadderModel { get; set; }

        public DbSet<Challenge> Challenge { get; set; }
        
        public DbSet<Booking> Booking { get; set; }
    }
}
