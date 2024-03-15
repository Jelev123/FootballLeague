namespace FootballLeague.Infrastructure.Models.OutputModels.Match
{
    using System;

    public class MatchOutputModels
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Result { get; set; }
        public DateTime Played { get; set; }
    }
}
