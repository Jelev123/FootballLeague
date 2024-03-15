namespace FootballLeague.Core.Interfaces.Match
{
    using FootballLeague.Infrastructure.Models.InputModels.Match;
    using FootballLeague.Infrastructure.Models.OutputModels.Match;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMatchService
    {
        Task CreateMatchAsync(CreateMatchModel model);
        Task<IEnumerable<MatchOutputModels>> AllMatchesAsync();
        Task<MatchOutputModels> GetmatchByIdAsync(int matchId);
        Task EditMatchAsync(EditMatchModel model, int teamId);
        Task DeleteMatchAsync(int teamId);
    }
}
