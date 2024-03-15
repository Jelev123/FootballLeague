namespace FootballLeague.Core.Services.Match
{
    using FootballLeague.Core.Interfaces.Match;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.Models.InputModels.Match;
    using FootballLeague.Infrastructure.Models.OutputModels.Match;
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

        public async Task CreateMatchAsync(CreateMatchModel model)
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
        }

        public async Task<IEnumerable<MatchOutputModels>> AllMatchesAsync()
        {
            var matches = await data.Matches
                         .Where(m => m.IsDeleted == false)
                         .Select(match => new MatchOutputModels
                         {
                             HomeTeam = match.HomeTeam.Name,
                             AwayTeam = match.AwayTeam.Name,
                             Result = $"{match.HomeTeam.Name} {match.HomeTeamGoals} - {match.AwayTeamGoals} {match.AwayTeam.Name}",
                             Played = match.CreatedOn
                         })
                         .ToListAsync();

            return matches;
        }

        public async Task<MatchOutputModels> GetmatchByIdAsync(int matchId)
        {
            var match = await data.Matches.Where(match => match.Id == matchId && match.IsDeleted == false)
            .Select(m => new MatchOutputModels
            {
                AwayTeam = m.AwayTeam.Name,
                HomeTeam = m.HomeTeam.Name,
                Result = $"{m.HomeTeam.Name} {m.HomeTeamGoals} - {m.AwayTeamGoals} {m.AwayTeam.Name}",
                Played = m.CreatedOn
            }).FirstOrDefaultAsync();

            return match;
        }

        public async Task EditMatchAsync(EditMatchModel model, int matchId)
        {
            var match = await data.Matches.FirstOrDefaultAsync(m => m.Id == matchId && !m.IsDeleted);

            match.AwayTeamId = model.AwayTeamId;
            match.AwayTeamGoals = model.AwayTeamGoals;
            match.HomeTeamId = model.HomeTeamId;
            match.HomeTeamGoals = model.HomeTeamGoals;
            match.ModifiedOn = DateTime.UtcNow;
            match.CreatedOn = model.Played;

            await data.SaveChangesAsync();
        }

        public async Task DeleteMatchAsync(int matchId)
        {
            var match = await data.Matches.FirstOrDefaultAsync(match => match.Id == matchId && !match.IsDeleted);

            if (match != null)
            {
                match.IsDeleted = true;
                match.ModifiedOn = DateTime.UtcNow;
                match.DeletedOn = DateTime.UtcNow;
                await data.SaveChangesAsync();
            } 
        }
    }
}
