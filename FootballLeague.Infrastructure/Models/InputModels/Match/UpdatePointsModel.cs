namespace FootballLeague.Infrastructure.Models.InputModels.Match
{
    public class UpdatePointsModel
    {  
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
    }
}
