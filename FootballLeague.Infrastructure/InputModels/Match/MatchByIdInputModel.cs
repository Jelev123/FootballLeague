namespace FootballLeague.Infrastructure.InputModels.Match
{
    using System;

    public class MatchByIdInputModel
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string Result { get; set; }
        public DateTime Played { get; set; }
    }
}
