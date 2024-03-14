namespace FootballLeague.Infrastructure.InputModels.Match
{
    using System;

    public class CreateMatchInputModel
    {
        public int AwayTeamId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamGoals { get; set; }
        public int HomeTeamGoals { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
