namespace FootballLeague.Infrastructure.InputModels.Match
{
    using Microsoft.VisualBasic;
    using System;

    public class EditMatchInputModel
    {
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public DateTime Played { get; set; }
    }
}
