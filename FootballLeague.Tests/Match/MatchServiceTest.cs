namespace FootballLeague.Tests.Match
{
    using FootballLeague.Core.Interfaces.Match;
    using FootballLeague.Core.Services.Match;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.Models.InputModels.Match;
    using FootballLeague.Tests.Mock;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MatchServiceTest
    {

        [Test]
        public async Task AllMatchesAsync_ShouldReturnAllMatches()
        {
            var matchService = await GetMatchServiceAsync();

            var result = await matchService.AllMatchesAsync();
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public async Task GetMatchByIdAsync_ShouldReturnMatch()
        {
            var matchService = await GetMatchServiceAsync();
            int matchId = 1;

            var result = await matchService.GetmatchByIdAsync(matchId);
            Assert.NotNull(result);
        }


        [Test]
        public async Task GetMatchByIdAsync_ShouldReturnNull_WhenMatchIdDoesNotExist()
        {
            var matchService = await GetMatchServiceAsync();
            int matchId = 999;

            var result = await matchService.GetmatchByIdAsync(matchId);
            Assert.Null(result);
        }


        [Test]
        public async Task EditMatchAsync_ShouldUpdateMatch()
        {
            var matchId = 1;
            var model = new EditMatchModel
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
            Assert.AreEqual(model.AwayTeamId, editedMatch.AwayTeamId);
            Assert.AreEqual(model.AwayTeamGoals, editedMatch.AwayTeamGoals);
            Assert.AreEqual(model.HomeTeamId, editedMatch.HomeTeamId);
            Assert.AreEqual(model.HomeTeamGoals, editedMatch.HomeTeamGoals);
            Assert.AreEqual(model.Played, editedMatch.CreatedOn);
        }


        [Test]
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


        [Test]
        public async Task DeleteMatchAsync_ShouldReturnFalse_WhenMatchIdDoesNotExist()
        {
            var dbContextMock = await GetDatabaseMockAsync();
            int notExistedMatchId = 999;

            var matchService = new MatchService(dbContextMock);

            await matchService.DeleteMatchAsync(notExistedMatchId);

            var deletedMatch = await dbContextMock.Matches.FindAsync(notExistedMatchId);
            Assert.Null(deletedMatch);
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
