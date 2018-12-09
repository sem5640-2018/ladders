using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ladders.Models
{
    public class Booking
    {
        [Key]
        public virtual int bookingId { get; set; }

        public virtual DateTime bookingDateTime { get; set; }

        public virtual string userId { get; set; }

        public virtual Facility facility { get; set; }
    }

    public class Facility
    {
        [Key]
        public virtual int facilityId { get; set; }

        public virtual string facilityName { get; set; }

        public virtual bool isBlock { get; set; }

        public virtual int venueId { get; set; }

        public virtual Venue venue { get; set; }

        public virtual int sportId { get; set; }

        public virtual Sport sport { get; set; }
    }

    public class Venue
    {
        [Key]
        public virtual int venueId { get; set; }

        public virtual string venueName { get; set; }
    }

    public class Sport
    {
        [Key]
        public virtual int sportId { get; set; }

        public virtual string sportName { get; set; }
    }
}
