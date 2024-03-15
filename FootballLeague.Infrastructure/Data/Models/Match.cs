namespace FootballLeague.Infrastructure.Data.Models
{
    using FootballLeague.Infrastructure.Models;

    public class Match : BaseDeletableModel<int>
    {
        public int AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public int AwayTeamGoals { get; set; }
        public int HomeTeamGoals { get; set; }
    }
}
