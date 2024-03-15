namespace FootballLeague.Infrastructure.Models.InputModels.Match
{
    using FootballLeague.Infrastructure.Constants.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EditMatchModel
    {
        [Range(Attributes.MinId,
            int.MaxValue,
            ErrorMessage = ValidateMessages.CorrectNumber)]
        public int HomeTeamId { get; set; }

        [Range(Attributes.MinId,
            int.MaxValue,
            ErrorMessage = ValidateMessages.CorrectNumber)]
        public int AwayTeamId { get; set; }

        [Range(Attributes.MinGoals,
            Attributes.MaxGoals,
            ErrorMessage = ValidateMessages.MinMaxGoals)]
        public int HomeTeamGoals { get; set; }

        [Range(Attributes.MinGoals,
            Attributes.MaxGoals,
            ErrorMessage = ValidateMessages.MinMaxGoals)]
        public int AwayTeamGoals { get; set; }

        public DateTime Played { get; set; }
    }
}
