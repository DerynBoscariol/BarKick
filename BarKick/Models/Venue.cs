using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarKick.Models
{
    public class Venue
    {
        [Key]
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }

        // Many teams play in venues
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<VenueBartender> VenueBartenders { get; set; }

        // Constructor to initialize collections
        public Venue()
        {
            Teams = new List<Team>();
            VenueBartenders = new List<VenueBartender>();
        }
    }

    public class VenueDto
    {
        public VenueDto()
        {
            Teams = new List<TeamDto>();
            Bartenders = new List<BartenderDto>();
        }

<<<<<<< Updated upstream
    public ICollection<TeamDto> Teams { get; set; }
    public List<VenueDto> Venues { get; set; }
=======
        public int VenueID { get; set; }
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }

        public ICollection<TeamDto> Teams { get; set; }
        public ICollection<BartenderDto> Bartenders { get; set; }
>>>>>>> Stashed changes
    }
}
