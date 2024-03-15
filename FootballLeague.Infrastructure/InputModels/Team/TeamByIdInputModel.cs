namespace FootballLeague.Infrastructure.InputModels.Team
{
    using System;

    public class TeamByIdInputModel
    {
        public string Name { get; set; }

        public int Points { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public int TotalMatchPlayed { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
