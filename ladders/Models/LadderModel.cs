using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ladders.Models
{
    public class LadderModel
    {
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [InverseProperty("LadderModel")]
        [MaxLength(200)]
        [Required]
        public virtual ICollection<Ranking> CurrentRankings { get; set; }

        [InverseProperty("ApprovalLadder")]
        [Required]
        public virtual ICollection<ProfileModel> ApprovalUsersList { get; set; }
    }
}
