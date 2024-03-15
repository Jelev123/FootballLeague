namespace FootballLeague.Infrastructure.Models.OutputModels.Team
{
    public class TeamRankingOutputModel
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public int Points { get; set; }
        public int TotalMatchesPlayed { get; set; }
        public int TotalGoalScored { get; set; }
        public int AwayMatchesPlayed { get; set; }
        public int HomeMatchesPlayed { get; set; }
        public int MatchesDraw { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
    }
}
