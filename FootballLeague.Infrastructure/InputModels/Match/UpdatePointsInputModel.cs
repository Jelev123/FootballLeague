namespace FootballLeague.Infrastructure.InputModels.Match
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdatePointsInputModel
    {  
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
