﻿namespace FootballLeague.Test.Match
{
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Core.Services.Match;
    using FootballLeague.Test.Mock;
    using FootballLeague.Infrastructure.InputModels.Match;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class MatchServiceTest
    {

        [Fact]
        public async Task CreateMatchAsync_ShouldCreateNewMatch()
        {
            var matchService = await GetMatchServiceAsync();
            var model = new CreateMatchInputModel
            {
                AwayTeamGoals = 2,
                AwayTeamId = 1, 
                HomeTeamGoals = 1,
                HomeTeamId = 2,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
            };

            await matchService.CreateMatchAsync(model);

            using (var context = await GetDatabaseMockAsync())
            {
                var createdMatch = await context.Matches.FirstOrDefaultAsync(m => m.HomeTeamId == model.HomeTeamId && m.AwayTeamId == model.AwayTeamId);
                Assert.NotNull(createdMatch);
                Assert.Equal(model.AwayTeamGoals, createdMatch.AwayTeamGoals);
                Assert.Equal(model.HomeTeamGoals, createdMatch.HomeTeamGoals);
            }
        }


        [Fact]
        public async Task AllMatchesAsync_ShouldReturnAllMatches()
        {
            var matchService = await GetMatchServiceAsync();

            var result = await matchService.AllMatchesAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }


        [Fact]
        public async Task GetMatchByIdAsync_ShouldReturnMatch()
        {
            var matchService = await GetMatchServiceAsync();
            int matchId = 1;

            var result = await matchService.GetmatchByIdAsync(matchId);
            Assert.NotNull(result);
        }


        [Fact]
        public async Task GetMatchByIdAsync_ShouldReturnNull_WhenMatchIdDoesNotExist()
        {
            var matchService = await GetMatchServiceAsync();
            int matchId = 999;

            var result = await matchService.GetmatchByIdAsync(matchId);
            Assert.Null(result);
        }


        [Fact]
        public async Task EditMatchAsync_ShouldUpdateMatch()
        {
            var matchId = 1;
            var model = new EditMatchInputModel
            {
                AwayTeamId = 2,
                AwayTeamGoals = 1,
                HomeTeamId = 3,
                HomeTeamGoals = 2,
                Played = DateTime.Now
            };

            var dbContextMock = await GetDatabaseMockAsync();

            var matchService = new MatchService(dbContextMock);

            await matchService.EditMatchAsync(model, matchId);

            var editedMatch = await dbContextMock.Matches.FindAsync(matchId);

            Assert.NotNull(editedMatch);
            Assert.Equal(model.AwayTeamId, editedMatch.AwayTeamId);
            Assert.Equal(model.AwayTeamGoals, editedMatch.AwayTeamGoals);
            Assert.Equal(model.HomeTeamId, editedMatch.HomeTeamId);
            Assert.Equal(model.HomeTeamGoals, editedMatch.HomeTeamGoals);
            Assert.Equal(model.Played, editedMatch.CreatedOn);
        }


        [Fact]
        public async Task DeleteMatchAsync_ShouldMarkMatchAsDeleted()
        {
            var dbContextMock = await GetDatabaseMockAsync();
            int matchId = 1;

            var matchService = new MatchService(dbContextMock);

            await matchService.DeleteMatchAsync(matchId);

            var deletedMatch = await dbContextMock.Matches.FindAsync(matchId);
            Assert.NotNull(deletedMatch);
            Assert.True(deletedMatch.IsDeleted);
        }


        [Fact]
        public async Task DeleteMatchAsync_ShouldReturnFalse_WhenMatchIdDoesNotExist()
        {
            var dbContextMock = await GetDatabaseMockAsync();
            int notExistedMatchId = 1;

            var matchService = new MatchService(dbContextMock);

            await matchService.DeleteMatchAsync(notExistedMatchId);

            var deletedMatch = await dbContextMock.Matches.FindAsync(notExistedMatchId);
            Assert.False(deletedMatch.IsDeleted);
        }


        private async Task<IMatchService> GetMatchServiceAsync()
        {
            var data = await GetDatabaseMockAsync();
            return new MatchService(data);
        }

        private async Task<ApplicationDbContext> GetDatabaseMockAsync()
        {
            var context = DbMock.Instance;

            var teams = new List<Team>()
            {
                new Team()
                {
                    Name = "Real Madrid",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Manchester United",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Barselona",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Inter",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Arsenal",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Manchester City",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Milan",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
                new Team()
                {
                    Name = "Chelsea",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                },
            };

            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();


            var matches = new List<Match>()
            {
                new Match()
                {
                    HomeTeamId= 1,
                    AwayTeamId= 2,
                    HomeTeamGoals = 4,
                    AwayTeamGoals = 1,
                },

                new Match()
                {
                    HomeTeamId= 2,
                    AwayTeamId= 5,
                    HomeTeamGoals = 3,
                    AwayTeamGoals = 2,
                },

                new Match()
                {
                    HomeTeamId= 1,
                    AwayTeamId= 7,
                    HomeTeamGoals = 1,
                    AwayTeamGoals = 1,
                },

                new Match()
                {
                    HomeTeamId= 3,
                    AwayTeamId= 4,
                    HomeTeamGoals = 5,
                    AwayTeamGoals = 5,
                },
            };

            await context.Matches.AddRangeAsync(matches);
            await context.SaveChangesAsync();

            return context;
        }
    }
}
