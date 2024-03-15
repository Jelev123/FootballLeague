namespace FootballLeague.Core.Contracts.Match
{
    using FootballLeague.Infrastructure.InputModels.Match;
    using FootballLeague.Infrastructure.InputModels.Team;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMatchService
    {
        Task<bool> CreateMatchAsync(CreateMatchInputModel model);
        Task<IEnumerable<AllMatchesInputModels>> AllMatchesAsync();
        Task<MatchByIdInputModel> GetmatchByIdAsync(int matchId);
        Task<bool> EditMatchAsync(EditMatchInputModel model, int teamId);
        Task<bool> DeleteMatchAsync(int teamId);
    }
}
