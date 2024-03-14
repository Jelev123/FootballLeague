namespace FootballLeague.Infrastructure.InputModels.Team
{
    using System;

    public class TeamByIdInputModel
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
