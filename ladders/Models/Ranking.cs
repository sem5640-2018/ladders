using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ladders.Models
{
    public class Ranking
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        private ProfileModel UserProfile { get; set; }

        [Required]
        private int Score { get; set; }
    }
}
