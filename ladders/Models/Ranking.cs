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
        public virtual int UserId { get; set; }

        [Required]
        public virtual int Wins { get; set; }

        [Required]
        public virtual int Losses { get; set; }

        [Required]
        public virtual int Draws { get; set; }

        public virtual int LadderModelId { get; set; }

        public virtual LadderModel LadderModel { get; set; }
    }
}
