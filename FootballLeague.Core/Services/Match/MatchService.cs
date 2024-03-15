namespace FootballLeague.Core.Services.Match
{
    using Amazon.CloudWatchLogs.Model;
    using FootballLeague.Core.Constants;
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.InputModels.Match;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext data;

        public MatchService(ApplicationDbContext data)
        {
            this.data = data;
        }


        public async Task<bool> CreateMatchAsync(CreateMatchInputModel model)
        {
            var validate = await ValidateMatchAsync(model.HomeTeamId, model.AwayTeamId);

            if (validate)
            {
                var match = new Match
                {
                    AwayTeamId = model.AwayTeamId,
                    AwayTeamGoals = model.AwayTeamGoals,
                    HomeTeamId = model.HomeTeamId,
                    HomeTeamGoals = model.HomeTeamGoals,
                    CreatedOn = model.CreatedOn,
                    ModifiedOn = model.LastModifiedOn,
                };

                await data.Matches.AddAsync(match);
                await data.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<AllMatchesInputModels>> AllMatchesAsync()
        {
            var matches = await data.Matches
                         .Where(m => m.IsDeleted == false)
                         .Select(match => new AllMatchesInputModels
                         {
                             HomeTeam = match.HomeTeam.Name,
                             AwayTeam = match.AwayTeam.Name,
                             Result = $"{match.HomeTeam.Name} {match.HomeTeamGoals} - {match.AwayTeamGoals} {match.AwayTeam.Name}",
                             Played = match.CreatedOn
                         })
                         .ToListAsync();

            if (matches == null || !matches.Any())
            {
                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Match).Name, "id", matches));
            }

            return matches;
        }

        public async Task<MatchByIdInputModel> GetmatchByIdAsync(int matchId)
        {
            var match = await data.Matches.Where(match => match.Id == matchId && match.IsDeleted == false)

            .Select(m => new MatchByIdInputModel
            {
                AwayTeamName = m.AwayTeam.Name,
                HomeTeamName = m.HomeTeam.Name,
                Result = $"{m.HomeTeam.Name} {m.HomeTeamGoals} - {m.AwayTeamGoals} {m.AwayTeam.Name}",
                Played = m.CreatedOn
            }).FirstOrDefaultAsync();

            if (match == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Match).Name, "id", matchId));
            }

            return match;
        }

        public async Task<bool> EditMatchAsync(EditMatchInputModel model, int matchId)
        {
            var match = await data.Matches.FirstOrDefaultAsync(m => m.Id == matchId && !m.IsDeleted);

            if (match == null)
            {
                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Match).Name, "id", matchId));
            }

            await ValidateMatchAsync(model.HomeTeamId, model.AwayTeamId);

            match.AwayTeamId = model.AwayTeamId;
            match.AwayTeamGoals = model.AwayTeamGoals;
            match.HomeTeamId = model.HomeTeamId;
            match.HomeTeamGoals = model.HomeTeamGoals;
            match.ModifiedOn = DateTime.UtcNow;
            match.CreatedOn = model.Played;

            await data.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMatchAsync(int matchId)
        {
            var match = await data.Matches.FirstOrDefaultAsync(match => match.Id == matchId);

            if (match == null)
            {
                throw new ResourceNotFoundException(string.Format(
                   ErrorMessages.DataDoesNotExist,
                   typeof(Match).Name, "id", matchId));
            }

            match.IsDeleted = true;
            match.ModifiedOn = DateTime.UtcNow;
            match.DeletedOn = DateTime.UtcNow;
            await data.SaveChangesAsync();
            return true;
        }

        public async Task UpdateTeamPointsAsync(UpdatePointsInputModel gameModel)
        {
            var homeTeamPoints = await CalculatePointsAsync(gameModel.HomeTeamId, gameModel.HomeTeamGoals, gameModel.AwayTeamGoals);
            var awayTeamPoints = await CalculatePointsAsync(gameModel.AwayTeamId, gameModel.AwayTeamGoals, gameModel.HomeTeamGoals);

            await UpdatePointsAsync(gameModel.HomeTeamId, homeTeamPoints);
            await UpdatePointsAsync(gameModel.AwayTeamId, awayTeamPoints);
        }

        private async Task<int> CalculatePointsAsync(int teamId, int teamGoals, int opponentGoals)
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

        private async Task<bool> ValidateMatchAsync(int homeTeamId, int awayTeamId)
        {
            if (homeTeamId == awayTeamId)
                throw new ArgumentException(string.Format(
                    ErrorMessages.SameTeams));

            var isAwayTeamExist = await data.Teams.AnyAsync(team => team.Id == awayTeamId && !team.IsDeleted);

            if (!isAwayTeamExist)
                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Team).Name, "id", awayTeamId));

            var isHomeTeamExist = await data.Teams.AnyAsync(team => team.Id == homeTeamId && !team.IsDeleted);

            if (!isHomeTeamExist)
                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Team).Name, "id", homeTeamId));

            return true;
        }

    }
}
