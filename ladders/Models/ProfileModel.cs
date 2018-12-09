using System.ComponentModel.DataAnnotations;

namespace ladders.Models
{
    public class ProfileModel
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string UserId { get; set; }

        [Required]
        public virtual string Availability { get; set; }

        [Required]
        public virtual string PreferredLocation { get; set; }

        [Required]
        public virtual bool Suspended { get; set; }

        public virtual string Name { get; set; }
        
        public virtual Ranking CurrentRanking { get; set; }
        public virtual int? CurrentRankingId { get; set; }

        public virtual LadderModel ApprovalLadder { get; set; }
        public virtual int? ApprovalLadderId { get; set; }

    }
}