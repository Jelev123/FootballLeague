namespace FootballLeague.Core.Services.Match
{
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Infrastructure.InputModels.Match;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MatchDecoratorService : IMatchService
    {
        private readonly MatchService matchService;

        public MatchDecoratorService(MatchService matchService)
        {
            this.matchService = matchService;
        }

        public async Task CreateMatchAsync(CreateMatchInputModel model)
        {
            // this is becouse when team is a guest the goals its multiple by 2 and that is important for ranking.
            model.AwayTeamGoals = model.AwayTeamGoals > 0 ? model.AwayTeamGoals * 2 : model.AwayTeamGoals;

            await matchService.CreateMatchAsync(model);

            var scoringUpdateModel = new UpdatePointsInputModel
            {
                HomeTeamId = model.HomeTeamId,
                HomeTeamGoals = model.HomeTeamGoals,
                AwayTeamId = model.AwayTeamId,
                AwayTeamGoals = model.AwayTeamGoals,
            };

            await matchService.UpdateTeamPointsAsync(scoringUpdateModel);
        }

        public Task<IEnumerable<AllMatchesInputModels>> AllMatchesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteMatchAsync(int teamId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> EditMatchAsync(EditMatchInputModel model, int teamId)
        {
            throw new System.NotImplementedException();
        }

        public Task<MatchByIdInputModel> GetmatchByIdAsync(int matchId)
        {
            throw new System.NotImplementedException();
        }
    }
}
