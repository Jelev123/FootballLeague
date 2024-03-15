namespace FootballLeague.Infrastructure.InputModels.Team
{
    using FootballLeague.Infrastructure.Constants.Attributes;
    using System.ComponentModel.DataAnnotations;


    public class CreateTeamInputModel
    {
        [Required(ErrorMessage = ValidateMessages.Required)]
        [StringLength(Attributes.TeamNameMaxLength,
            ErrorMessage = ValidateMessages.MinMaxLength,
            MinimumLength = Attributes.TeamNameMinLength)]
        public string TeamName { get; set; }
    }
}
