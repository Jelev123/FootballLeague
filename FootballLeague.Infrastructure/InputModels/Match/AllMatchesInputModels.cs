namespace FootballLeague.Infrastructure.InputModels.Match
{
    using System;

    public class AllMatchesInputModels
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Result { get; set; }
        public DateTime Played { get; set; }
    }
}
