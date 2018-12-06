using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

        public virtual int CurrentLadder { get; set; }
    }
}