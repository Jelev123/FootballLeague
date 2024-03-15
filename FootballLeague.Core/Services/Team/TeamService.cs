﻿namespace FootballLeague.Core.Services.Team
{
    using Amazon.CloudWatchLogs.Model;
    using FootballLeague.Core.Constants;
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.InputModels.Team;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext data;

        public TeamService(ApplicationDbContext data)
        {
            this.data = data;
        }

        public async Task CreateAsync(CreateTeamInputModel model)
        {
            bool isNameAlReadyExist = await IsTheNameAlreadyExist(model.TeamName);

            if (isNameAlReadyExist)
            {
                throw new ResourceAlreadyExistsException(string.Format(
                    ErrorMessages.DataAlreadyExists,
                    typeof(Team).Name, model.TeamName));
            }

            var team = new Team
            {
                Name = model.TeamName,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
            };

            await data.Teams.AddAsync(team);
            await data.SaveChangesAsync();
        }


        public async Task<IEnumerable<AllTeamsModel>> GetAllTeamsAsync()
        {
            return await data.Teams
                      .Where(team => !team.IsDeleted)
                      .Select(t => new AllTeamsModel
                      {
                          Name = t.Name,
                          CreatedOn = t.CreatedOn,
                          ModifiedOn = (DateTime)t.ModifiedOn,
                      }).ToListAsync();
        }

        public async Task<TeamByIdInputModel> GetTeamById(int teamId)
        {
            return await data.Teams.Where(team => team.Id == teamId && team.IsDeleted == false)
            .Select(t => new TeamByIdInputModel
            {
                Name = t.Name,
                Points = t.Points,
                HomeGoals = t.HomeMatches.Sum(g => g.HomeTeamGoals),
                AwayGoals = t.AwayMatches.Sum(g => g.AwayTeamGoals),
                TotalMatchPlayed = t.HomeMatches.Count() + t.AwayMatches.Count(),
                CreatedOn = t.CreatedOn,
                ModifiedOn = t.ModifiedOn,
            }).FirstOrDefaultAsync();

        }
        public async Task<bool> EditTeamAsync(EditTeamInputModel model, int teamId)
        {
            var team = await data.Teams.FirstOrDefaultAsync(t => t.Id == teamId);

            if (team != null)
            {
                team.Name = model.Name;
                team.ModifiedOn = DateTime.UtcNow;

                await data.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var team = await data.Teams
                .Where(team => !team.IsDeleted)
                .FirstOrDefaultAsync(team => team.Id == teamId);

            if (team != null)
            {
                team.IsDeleted = true;
                team.ModifiedOn = DateTime.UtcNow;
                team.DeletedOn = DateTime.UtcNow;
                await data.SaveChangesAsync();
                return true;
            }

            throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Team).Name, "id", teamId));
        }

        public async Task<bool> IsTheNameAlreadyExist(string teamName)
        {
            return await data.Teams.AnyAsync(t => t.Name == teamName);
        }

        public async Task<ICollection<AllTeamsRanking>> GetAllTeamsRankingAsync()
        {
            var teamsRanking = await GetTeamsWithRankingDataAsync();
            OrderTeamsByRanking(ref teamsRanking);
            SetRankingPositions(ref teamsRanking);
            return teamsRanking;
        }

        private async Task<ICollection<AllTeamsRanking>> GetTeamsWithRankingDataAsync()
        {
            return await data.Teams
                .Where(team => !team.IsDeleted)
                .Select(team => new AllTeamsRanking
                {
                    Id = team.Id,
                    Name = team.Name,
                    Points = team.Points,
                    HomeMatchesPlayed = team.HomeMatches.Count(),
                    AwayMatchesPlayed = team.AwayMatches.Count(),
                    MatchesWon = team.HomeMatches.Count(x => x.HomeTeamGoals > x.AwayTeamGoals) +
                                  team.AwayMatches.Count(x => x.AwayTeamGoals > x.HomeTeamGoals),
                    MatchesLost = team.HomeMatches.Count(x => x.HomeTeamGoals < x.AwayTeamGoals) +
                                  team.AwayMatches.Count(x => x.AwayTeamGoals < x.HomeTeamGoals),
                    MatchesDraw = team.HomeMatches.Count(x => x.HomeTeamGoals == x.AwayTeamGoals) +
                                  team.AwayMatches.Count(x => x.AwayTeamGoals == x.HomeTeamGoals),
                    TotalMatchesPlayed = team.HomeMatches.Count() + team.AwayMatches.Count(),
                    TotalGoalScored = team.HomeMatches.Sum(x => x.HomeTeamGoals) +
                                  team.AwayMatches.Sum(x => x.AwayTeamGoals),
                })
                .ToListAsync();
        }

        private void OrderTeamsByRanking(ref ICollection<AllTeamsRanking> teamsRanking)
        {
            teamsRanking = teamsRanking.OrderByDescending(team => team.Points)
                                       .ThenByDescending(team => team.TotalGoalScored)
                                       .ThenByDescending(team => team.MatchesWon)
                                       .ThenByDescending(team => team.MatchesDraw)
                                       .ToList();
        }

        private void SetRankingPositions(ref ICollection<AllTeamsRanking> teamsRanking)
        {
            var index = 1;
            foreach (var team in teamsRanking)
            {
                team.Rank = index++;
            }
        }
    }
}
