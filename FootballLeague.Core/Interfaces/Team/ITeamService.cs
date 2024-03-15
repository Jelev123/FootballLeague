namespace FootballLeague.Core.Interfaces.Team
{
    using FootballLeague.Infrastructure.Models.InputModels.Team;
    using FootballLeague.Infrastructure.Models.OutputModels.Team;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task CreateAsync(CreateTeamModel model);
        Task<IEnumerable<AllTeamsOutputModel>> GetAllTeamsAsync();
        Task<ICollection<TeamRankingOutputModel>> GetAllTeamsRankingAsync();
        Task<TeamByIdOutputModel> GetTeamByIdAsync(int teamId);
        Task EditTeamAsync(EditTeamInputModel model, int teamId);
        Task DeleteTeamAsync(int teamId);
    }
}
