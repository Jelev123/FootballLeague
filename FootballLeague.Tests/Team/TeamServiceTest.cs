namespace FootballLeague.Tests.Team
{
    using FootballLeague.Core.Interfaces.Team;
    using FootballLeague.Core.Services.Team;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.Models.InputModels.Team;
    using FootballLeague.Infrastructure.Models.OutputModels.Team;
    using FootballLeague.Tests.Mock;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeamServiceTest
    {
        [Test]
        public async Task GetAllTeams_ShouldReturn_AllTeams()
        {

            var teamService = await GetTeamServiceAsync();

            var result = await teamService.GetAllTeamsAsync();

            Assert.IsNotEmpty(result);
            Assert.AreEqual(8, result.ToList().Count);
        }

        [Test]
        public async Task GetTeamById_ShouldReturn_Team()
        {

            var teamService = await GetTeamServiceAsync();

            var result = await teamService.GetTeamByIdAsync(1);

            Assert.IsInstanceOf<TeamByIdOutputModel>(result);
            Assert.AreEqual("Real Madrid", result.Name);
        }

        [Test]
        public async Task EditTeamAsync_ShouldUpdateTeamName()
        {
            int teamId = 1;
            var model = new EditTeamInputModel
            {
                TeamName = "New Team Name"
            };

            var dbContextMock = await GetDatabaseMockAsync();

            var teamService = new TeamService(dbContextMock);

            await teamService.EditTeamAsync(model, teamId);

            var updatedTeam = await dbContextMock.Teams.FindAsync(teamId);

            Assert.NotNull(updatedTeam);
            Assert.AreEqual(model.TeamName, updatedTeam.Name);
            Assert.NotNull(updatedTeam.ModifiedOn);
        }

        [Test]
        public async Task EditTeamAsync_ShouldReturnFalse_IfTeamNotFound()
        {
            int teamId = 999;

            var model = new EditTeamInputModel
            {
                TeamName = "New Team Name"
            };

            var dbContextMock = await GetDatabaseMockAsync();

            var teamService = new TeamService(dbContextMock);

            await teamService.EditTeamAsync(model, teamId);


            var updatedTeam = await dbContextMock.Teams.FindAsync(teamId);

            Assert.Null(updatedTeam);
        }

        private async Task<ITeamService> GetTeamServiceAsync()
        {
            var data = await GetDatabaseMockAsync();
            return new TeamService(data);
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

            return context;
        }
    }
}
