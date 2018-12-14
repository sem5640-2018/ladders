using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ladders.Models;

namespace ladders.Models
{
    public class LaddersContext : DbContext
    {
        public LaddersContext (DbContextOptions<LaddersContext> options)
            : base(options)
        {
        }

        public DbSet<ladders.Models.ProfileModel> ProfileModel { get; set; }

        public DbSet<ladders.Models.LadderModel> LadderModel { get; set; }

        public DbSet<ladders.Models.Challenge> Challenge { get; set; }
        
        public DbSet<ladders.Models.Booking> Booking { get; set; }
    }
}
