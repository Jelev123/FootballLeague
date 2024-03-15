namespace FootballLeague.Core.Services.Match
{
    using Amazon.CloudWatchLogs.Model;
    using FootballLeague.Core.Constants;
    using FootballLeague.Core.Constants.Logger;
    using FootballLeague.Infrastructure.Constants.Logger.Match;
    using FootballLeague.Core.Contracts.Loger;
    using FootballLeague.Core.Interfaces.Match;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.Models.InputModels.Match;
    using FootballLeague.Infrastructure.Models.OutputModels.Match;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MatchDecoratorService : IMatchService
    {
        private readonly MatchService matchService;
        private readonly ILoggerService loggerService;
        private readonly ApplicationDbContext data;

        public MatchDecoratorService(MatchService matchService, ILoggerService loggerService, ApplicationDbContext data)
        {
            this.matchService = matchService;
            this.loggerService = loggerService;
            this.data = data;
        }

        public async Task CreateMatchAsync(CreateMatchModel model)
        {
            loggerService.LogInfo(MatchRequestType.CreateMatch.ToString());

            await this.ValidateMatchAsync(model.HomeTeamId, model.AwayTeamId);

            await matchService.CreateMatchAsync(model);

            loggerService.LogInfo(RequestStatus.Success.ToString());

            var scoringUpdateModel = new UpdatePointsModel
            {
                HomeTeamId = model.HomeTeamId,
                HomeTeamGoals = model.HomeTeamGoals,
                AwayTeamId = model.AwayTeamId,
                AwayTeamGoals = model.AwayTeamGoals,
            };

            await this.UpdateTeamPointsAsync(scoringUpdateModel);
        }

        public async Task<IEnumerable<MatchOutputModels>> AllMatchesAsync()
        {
            loggerService.LogInfo(MatchRequestType.GetAllMatches.ToString());

            var teams = await matchService.AllMatchesAsync();

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return teams;
        }

        public async Task DeleteMatchAsync(int matchId)
        {
            loggerService.LogInfo(MatchRequestType.DeleteMatch.ToString());

            var match = await data.Matches.FirstOrDefaultAsync(match => match.Id == matchId && !match.IsDeleted);

            if (match == null)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new ResourceNotFoundException(string.Format(
                   ErrorMessages.DataDoesNotExist,
                   typeof(Match).Name, "id", matchId));
            }

            await matchService.DeleteMatchAsync(matchId);
            loggerService.LogInfo(RequestStatus.Success.ToString());
        }

        public async Task EditMatchAsync(EditMatchModel model, int matchId)
        {
            loggerService.LogInfo(MatchRequestType.UpdateMatch.ToString());

            var match = await data.Matches.FirstOrDefaultAsync(m => m.Id == matchId && !m.IsDeleted);

            if (match == null)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Match).Name, "id", matchId));
            }

            await this.ValidateMatchAsync(model.HomeTeamId, model.AwayTeamId);

            loggerService.LogInfo(RequestStatus.Success.ToString());

            await matchService.EditMatchAsync(model, matchId);
        }

        public async Task<MatchOutputModels> GetmatchByIdAsync(int matchId)
        {
            loggerService.LogInfo(MatchRequestType.GetMatch.ToString());

            var matchById = await matchService.GetmatchByIdAsync(matchId);

            if (matchById == null)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Match).Name, "id", matchId));
            }

            loggerService.LogInfo(RequestStatus.Success.ToString());
            return matchById;
        }

        private async Task ValidateMatchAsync(int homeTeamId, int awayTeamId)
        {
            if (homeTeamId == awayTeamId)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new ArgumentException(string.Format(
                        ErrorMessages.SameTeams));
            }

            var isAwayTeamExist = await data.Teams.AnyAsync(team => team.Id == awayTeamId && !team.IsDeleted);

            if (!isAwayTeamExist)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new ResourceNotFoundException(string.Format(
                        ErrorMessages.DataDoesNotExist,
                        typeof(Team).Name, "id", awayTeamId));
            }

            var isHomeTeamExist = await data.Teams.AnyAsync(team => team.Id == homeTeamId && !team.IsDeleted);

            if (!isHomeTeamExist)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new ResourceNotFoundException(string.Format(
                        ErrorMessages.DataDoesNotExist,
                        typeof(Team).Name, "id", homeTeamId));
            }
        }

        private async Task UpdateTeamPointsAsync(UpdatePointsModel gameModel)
        {
            var homeTeamPoints =  CalculatePointsAsync(gameModel.HomeTeamId, gameModel.HomeTeamGoals, gameModel.AwayTeamGoals);
            var awayTeamPoints =  CalculatePointsAsync(gameModel.AwayTeamId, gameModel.AwayTeamGoals, gameModel.HomeTeamGoals);

            await UpdatePointsAsync(gameModel.HomeTeamId, homeTeamPoints);
            await UpdatePointsAsync(gameModel.AwayTeamId, awayTeamPoints);
        }

        private int CalculatePointsAsync(int teamId, int teamGoals, int opponentGoals)
        {
            if (teamGoals > opponentGoals)
            {
                return ScoringPoints.Win;
            }
            else if (teamGoals == opponentGoals)
            {
                return ScoringPoints.Draw;
            }
            else
            {
                return ScoringPoints.Loss;
            }
        }

        private async Task UpdatePointsAsync(int teamId, int points)
        {
            var team = await data.Teams.FindAsync(teamId);

            if (team == null)
            {
                throw new ResourceNotFoundException(string.Format(
                ErrorMessages.DataDoesNotExist,
                   typeof(Team).Name, "id", teamId));
            }

            team.Points += points;
            team.ModifiedOn = DateTime.UtcNow;
            await data.SaveChangesAsync();
        }
    }
}
