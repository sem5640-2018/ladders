using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ladders.Models
{
    public enum Winner
    {
        Challenger = 1,
        Draw = 0,
        Challengee = -1
    }

    public class Challenge
    {
        [Key]
        [Required]
        public virtual int Id { get; set; }

        public int? ChallengerId { get; set; }
        
        [ForeignKey("ChallengerId")]
        public virtual ProfileModel Challenger { get; set; }

        public int? ChallengeeId { get; set; }
        
        [ForeignKey("ChallengeeId")]
        public virtual ProfileModel Challengee { get; set; }
        
        public virtual LadderModel Ladder { get; set; }

        [Required]
        public virtual int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        [Required]
        public virtual DateTime ChallengedTime { get; set; }

        public virtual DateTime Created { get; set; }

        [Required]
        public virtual bool Resolved { get; set; }

        public virtual Winner Result { get; set; }
    }
}
