namespace FootballLeague.Infrastructure.Models.InputModels.Team
{
    using FootballLeague.Infrastructure.Constants.Attributes;
    using System.ComponentModel.DataAnnotations;

    public class EditTeamInputModel
    {
        [Required(ErrorMessage = ValidateMessages.Required)]
        [StringLength(Attributes.TeamNameMaxLength,
            ErrorMessage = ValidateMessages.MinMaxLength,
            MinimumLength = Attributes.TeamNameMinLength)]
        public string TeamName { get; set; }
    }
}
