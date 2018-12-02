using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ladders.Models
{
    public class LaddersContext : DbContext
    {
        public LaddersContext (DbContextOptions<LaddersContext> options)
            : base(options)
        {
        }

        public DbSet<ladders.Models.ProfileModel> ProfileModel { get; set; }
    }
}
