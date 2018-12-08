using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ladders.Models
{
    public enum Winner
    {
        Challenger = 1,
        Draw = 1,
        Challengee = -1
    }

    public class Challenge
    {
        [Key]
        public virtual int Id { get; set; }
        
        public virtual ProfileModel Challenger { get; set; }
        
        public virtual ProfileModel Challengee { get; set; }
        
        public virtual LadderModel Ladder { get; set; }

        [Required]
        public virtual int BookingId { get; set; }

        public virtual Facility Facility { get; set; }

        [Required]
        public virtual DateTime ChallengedTime { get; set; }

        [Required]
        public virtual bool Resolved { get; set; }

        public virtual Winner Result { get; set; }
    }
}
