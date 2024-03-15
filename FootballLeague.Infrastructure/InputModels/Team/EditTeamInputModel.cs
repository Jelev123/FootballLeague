namespace FootballLeague.Infrastructure.InputModels.Team
{
    using FootballLeague.Core.Constants.Attributes;
    using System.ComponentModel.DataAnnotations;

    public class EditTeamInputModel
    {
        [Required(ErrorMessage = ValidateMessages.Required)]
        [StringLength(Attributes.TeamNameMaxLength,
            ErrorMessage = ValidateMessages.MinMaxLength,
            MinimumLength = Attributes.TeamNameMinLength)]
        public string Name { get; set; }
    }
}
