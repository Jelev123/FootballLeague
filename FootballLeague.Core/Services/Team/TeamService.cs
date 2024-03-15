namespace FootballLeague.Core.Services.Team
{
    using FootballLeague.Core.Interfaces.Team;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.Models.InputModels.Team;
    using FootballLeague.Infrastructure.Models.OutputModels.Team;
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

        public async Task CreateAsync(CreateTeamModel model)
        {
            var team = new Team
            {
                Name = model.TeamName,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
            };

            await data.Teams.AddAsync(team);
            await data.SaveChangesAsync();
        }


        public async Task<IEnumerable<AllTeamsOutputModel>> GetAllTeamsAsync()
        {
            return await data.Teams
                      .Where(team => !team.IsDeleted)
                      .Select(t => new AllTeamsOutputModel
                      {
                          Name = t.Name,
                          CreatedOn = t.CreatedOn,
                          ModifiedOn = (DateTime)t.ModifiedOn,
                      }).ToListAsync();
        }

        public async Task<TeamByIdOutputModel> GetTeamByIdAsync(int teamId)
        {
            return await data.Teams.Where(team => team.Id == teamId && team.IsDeleted == false)
            .Select(t => new TeamByIdOutputModel
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

        public async Task EditTeamAsync(EditTeamInputModel model, int teamId)
        {
            var team = await data.Teams.FirstOrDefaultAsync(t => t.Id == teamId);

            if (team != null)
            {
                team.Name = model.TeamName;
                team.ModifiedOn = DateTime.UtcNow;

                await data.SaveChangesAsync();
            }
        }

        public async Task DeleteTeamAsync(int teamId)
        {
            var team = await data.Teams
                .Where(team => !team.IsDeleted)
                .FirstOrDefaultAsync(team => team.Id == teamId);


            team.IsDeleted = true;
            team.ModifiedOn = DateTime.UtcNow;
            team.DeletedOn = DateTime.UtcNow;
            await data.SaveChangesAsync();
        }

        public async Task<ICollection<TeamRankingOutputModel>> GetAllTeamsRankingAsync()
        {
            var teamsRanking = await data.Teams
                .Where(team => !team.IsDeleted)
                .Select(team => new TeamRankingOutputModel
                {
                    Name = team.Name,
                    Points = team.Points,
                    HomeMatchesPlayed = team.HomeMatches.Count(),
                    AwayMatchesPlayed = team.AwayMatches.Count(),
                    MatchesWon = team.HomeMatches.Count(t => t.HomeTeamGoals > t.AwayTeamGoals) +
                                 team.AwayMatches.Count(t => t.AwayTeamGoals > t.HomeTeamGoals),
                    MatchesDraw = team.HomeMatches.Count(x => x.HomeTeamGoals == x.AwayTeamGoals) +
                                  team.AwayMatches.Count(t => t.AwayTeamGoals == t.HomeTeamGoals),
                    MatchesLost = team.HomeMatches.Count(x => x.HomeTeamGoals < x.AwayTeamGoals) +
                                  team.AwayMatches.Count(t => t.AwayTeamGoals < t.HomeTeamGoals),
                    TotalMatchesPlayed = team.HomeMatches.Count() + team.AwayMatches.Count(),
                    TotalGoalScored = team.HomeMatches.Sum(t => t.HomeTeamGoals) +
                                  team.AwayMatches.Sum(t => t.AwayTeamGoals),
                })
                 .OrderByDescending(team => team.Points)
                 .ThenByDescending(team => team.TotalGoalScored)
                 .ThenByDescending(team => team.MatchesWon)
                 .ThenByDescending(team => team.MatchesDraw)
                 .ToListAsync();

            await Task.Run(() => SetRankingPositions(teamsRanking));

            return teamsRanking;
        }

        private void SetRankingPositions(ICollection<TeamRankingOutputModel> teamsRanking)
        {
            var index = 1;
            foreach (var team in teamsRanking)
            {
                team.Rank = index++;
            }
        }
    }
}
