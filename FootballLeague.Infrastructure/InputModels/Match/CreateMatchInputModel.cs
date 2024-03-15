namespace FootballLeague.Infrastructure.InputModels.Match
{
    using FootballLeague.Core.Constants.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateMatchInputModel
    {

        [Range(Attributes.MinId,
            int.MaxValue,
            ErrorMessage = ValidateMessages.CorrectNumber)]
        public int AwayTeamId { get; set; }

        [Range(Attributes.MinId,
            int.MaxValue,
            ErrorMessage = ValidateMessages.CorrectNumber)]
        public int HomeTeamId { get; set; }

        [Range(Attributes.MinGoals,
            Attributes.MaxGoals,
            ErrorMessage = ValidateMessages.MinMaxGoals)]
        public int AwayTeamGoals { get; set; }

        [Range(Attributes.MinGoals,
            Attributes.MaxGoals,
            ErrorMessage = ValidateMessages.MinMaxGoals)]
        public int HomeTeamGoals { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }
    }
}
