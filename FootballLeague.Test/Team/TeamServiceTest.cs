namespace FootballLeague.Test.Team
{
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Core.Services.Team;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.InputModels.Team;
    using FootballLeague.Test.Mock;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class TeamServiceTest
    {


        [Fact]
        public async Task CreateAsync_ShouldCreateNewTeam_WhenNameIsNotInUse()
        {
            var teamService = await GetTeamServiceAsync();
            var model = new CreateTeamInputModel { TeamName = "Liverpool" };

            await teamService.CreateAsync(model);

            using (var context = await GetDatabaseMockAsync())
            {
                var createdTeam = await context.Teams.FirstOrDefaultAsync(t => t.Name == model.TeamName);
                Assert.NotNull(createdTeam);
            }
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenNameIsAlreadyInUse()
        {
            var teamService = await GetTeamServiceAsync();
            var existingTeamName = "Liverpool";
            var model = new CreateTeamInputModel { TeamName = existingTeamName };

            await Assert.ThrowsAsync<ArgumentException>(async () => await teamService.CreateAsync(model));
        }


        [Fact]
        public async Task GetAllTeams_ShouldReturn_AllTeams()
        {

            var teamService = await GetTeamServiceAsync();


            var result = await teamService.GetAllTeamsAsync();

            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<ICollection<AllTeamsModel>>(result);
            Assert.Equal(8, result.ToList().Count);
        }

        [Fact]
        public async Task GetTeamById_ShouldReturn_Team()
        {

            var teamService = await GetTeamServiceAsync();


            var result = await teamService.GetTeamById(1);

            Assert.IsType<TeamByIdInputModel>(result);
            Assert.Equal("Real Madrid", result.Name);
        }


        [Fact]
        public async Task EditTeamAsync_ShouldUpdateTeamName()
        {
            var teamService = await GetTeamServiceAsync();
            var teamIdToUpdate = 1;
            var newName = "Real Madrid 2";

            var result = await teamService.EditTeamAsync(new EditTeamInputModel { Name = newName }, teamIdToUpdate);

            Assert.True(result);

            using (var context = await GetDatabaseMockAsync())
            {
                var updatedTeam = await context.Teams.FindAsync(teamIdToUpdate);
                Assert.NotNull(updatedTeam);
                Assert.Equal(newName, updatedTeam.Name);
            }
        }

        [Fact]
        public async Task EditTeamAsync_ShouldReturnFalse_IfTeamNotFound()
        {
            var teamService = await GetTeamServiceAsync();
            var nonExistingTeamId = 100;
            var newName = "Test Team";

            var result = await teamService.EditTeamAsync(new EditTeamInputModel { Name = newName }, nonExistingTeamId);

            Assert.False(result);
        }


        [Fact]
        public async Task DeleteTeamAsync_ShouldDeleteTeam()
        {
            var teamService = await GetTeamServiceAsync();
            var teamIdToDelete = 1; 

            var result = await teamService.DeleteTeamAsync(teamIdToDelete);

            Assert.True(result);

            using (var context = await GetDatabaseMockAsync())
            {
                var deletedTeam = await context.Teams.FindAsync(teamIdToDelete);
                Assert.NotNull(deletedTeam);
                Assert.True(deletedTeam.IsDeleted);
            }
        }

        [Fact]
        public async Task DeleteTeamAsync_ShouldReturnFalse_IfTeamNotFound()
        {

            var teamService = await GetTeamServiceAsync();
            var nonExistingTeamId = 100;

            var result = await teamService.DeleteTeamAsync(nonExistingTeamId);

            Assert.False(result);
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
