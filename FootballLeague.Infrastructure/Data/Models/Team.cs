namespace FootballLeague.Infrastructure.Data.Models
{
    using FootballLeague.Infrastructure.Models;
    using System.Collections.Generic;

    public class Team : BaseDeletableModel<int>
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public virtual ICollection<Match> HomeMatches { get; set; } = new HashSet<Match>();
        public virtual ICollection<Match> AwayMatches { get; set; } = new HashSet<Match>();
    }
}
