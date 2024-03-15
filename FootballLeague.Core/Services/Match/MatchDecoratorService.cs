namespace FootballLeague.Core.Services.Match
{
    using FootballLeague.Core.Constants.Logger;
    using FootballLeague.Core.Constants.Logger.Match;
    using FootballLeague.Core.Contracts.Loger;
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Infrastructure.InputModels.Match;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MatchDecoratorService : IMatchService
    {
        private readonly MatchService matchService;
        private readonly ILoggerService loggerService;

        public MatchDecoratorService(MatchService matchService, ILoggerService loggerService)
        {
            this.matchService = matchService;
            this.loggerService = loggerService;
        }

        public async Task<bool> CreateMatchAsync(CreateMatchInputModel model)
        {
            loggerService.LogInfo(MatchRequestType.CreateMatch.ToString());

            // this is becouse when team is a guest the goals its multiple by 2 and that is important for ranking.
            model.AwayTeamGoals = model.AwayTeamGoals > 0 ? model.AwayTeamGoals * 2 : model.AwayTeamGoals;

            var isCreated = await matchService.CreateMatchAsync(model);

            if (isCreated)
            {
                loggerService.LogInfo(RequestStatus.Success.ToString());

                var scoringUpdateModel = new UpdatePointsInputModel
                {
                    HomeTeamId = model.HomeTeamId,
                    HomeTeamGoals = model.HomeTeamGoals,
                    AwayTeamId = model.AwayTeamId,
                    AwayTeamGoals = model.AwayTeamGoals,
                };

                await matchService.UpdateTeamPointsAsync(scoringUpdateModel);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<AllMatchesInputModels>> AllMatchesAsync()
        {
            loggerService.LogInfo(MatchRequestType.GetAllMatches.ToString());

            var teams = await matchService.AllMatchesAsync();

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return teams;
        }

        public async Task<bool> DeleteMatchAsync(int teamId)
        {
            loggerService.LogInfo(MatchRequestType.DeleteMatch.ToString());

            if (await matchService.DeleteMatchAsync(teamId))
            {
                loggerService.LogInfo(RequestStatus.Success.ToString());

                return true;
            }
            else
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                return false;
            }
        }

        public async Task<bool> EditMatchAsync(EditMatchInputModel model, int teamId)
        {
            loggerService.LogInfo(MatchRequestType.UpdateMatch.ToString());

            model.AwayTeamGoals = model.AwayTeamGoals > 0 ? model.AwayTeamGoals * 2 : model.AwayTeamGoals;

            bool editSuccess = await matchService.EditMatchAsync(model, teamId);

            if (editSuccess)
            {
                loggerService.LogInfo(RequestStatus.Success.ToString());
                return true;
            }
            else
            {
                loggerService.LogError(RequestStatus.Failed.ToString());
                return false;
            }
        }

        public async Task<MatchByIdInputModel> GetmatchByIdAsync(int matchId)
        {
            loggerService.LogInfo(MatchRequestType.GetMatch.ToString());

            var team = await matchService.GetmatchByIdAsync(matchId);

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return team;
        }
    }
}
