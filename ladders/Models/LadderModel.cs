using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ladders.Models
{
    public class LadderModel
    {
        [Key]
        public virtual int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public virtual List<ProfileModel> MemberList { get; set; }

        [Required]
        public virtual List<Ranking> CurrentRankings { get; set; }

        [Required]
        public virtual List<ProfileModel> ApprovalUsersList { get; set; }
    }
}
