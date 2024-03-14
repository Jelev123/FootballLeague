namespace FootballLeague.Core.Contracts.Team
{
    using FootballLeague.Infrastructure.InputModels.Team;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task<bool> IsTeamNameInUseAsync(string teamName);
        Task CreateAsync(CreateTeamInputModel model);
        Task<IEnumerable<AllTeamsModel>> GetAllTeamsAsync();
        Task<ICollection<AllTeamsRanking>> GetAllTeamsRankingAsync();
        Task<TeamByIdInputModel> GetTeamById(int teamId);
        Task<bool> EditTeamAsync(EditTeamInputModel model, int teamId);
        Task<bool> DeleteTeamAsync(int teamId);
    }
}
