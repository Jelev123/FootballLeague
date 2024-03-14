namespace FootballLeague.Core.Services.Team
{
    using FootballLeague.Core.Contracts.Loger;
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Infrastructure.InputModels.Team;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TeamDecoratorService : ITeamService
    {
        private readonly TeamService teamService;
        private readonly ILoggerService loggerService;

        public TeamDecoratorService(TeamService teamService, ILoggerService loggerService)
        {
            this.teamService = teamService;
            this.loggerService = loggerService;
        }

        public async Task CreateAsync(CreateTeamInputModel model)
        {

            loggerService.LogInfo("Creating team...");
            bool isInUse = await this.IsTeamNameInUseAsync(model.TeamName);
            if (isInUse)
            {
                await teamService.CreateAsync(model);
                loggerService.LogInfo("Team created successfully.");
            }
            else
            {
                loggerService.LogError($"Failed to create team");
            }
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            try
            {
                loggerService.LogInfo("Deleting team...");
                await teamService.DeleteTeamAsync(teamId);
                loggerService.LogInfo("Team deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed to delete team: {ex.Message}");
                return false;
            }

        }

        public async Task<bool> EditTeamAsync(EditTeamInputModel model, int teamId)
        {
            try
            {
                loggerService.LogInfo("Editing team...");
                bool editSuccess = await teamService.EditTeamAsync(model, teamId);
                if (editSuccess)
                {
                    loggerService.LogInfo("Team edited successfully.");
                    return true;
                }
                else
                {
                    loggerService.LogError($"Failed to edit team: Team not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed to edit team: {ex.Message}");
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<AllTeamsModel>> GetAllTeamsAsync()
        {
            try
            {
                loggerService.LogInfo("Getting teams...");
                var teams = await teamService.GetAllTeamsAsync();
                loggerService.LogInfo("All teams were retrieved successfully.");
                return teams;
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed to get teams: {ex.Message}");
                throw;
            }

            return null;
        }

        public async Task<ICollection<AllTeamsRanking>> GetAllTeamsRankingAsync()
        {
            try
            {
                loggerService.LogInfo("Getting teams...");
                var teams = await teamService.GetAllTeamsRankingAsync();
                loggerService.LogInfo("All teams by rank were retrieved successfully.");
                return teams;
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed to get teams: {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<TeamByIdInputModel> GetTeamById(int teamId)
        {
            loggerService.LogInfo("Getting team...");
            var team = await teamService.GetTeamById(teamId);
            loggerService.LogInfo("Team were got successfully.");
            return team;
        }

        public async Task<bool> IsTeamNameInUseAsync(string teamName)
        {
            try
            {

                loggerService.LogInfo("Checking the team name");
                bool isInUse = await teamService.IsTeamNameInUseAsync(teamName);
                if (!isInUse)
                {
                    loggerService.LogInfo("The name is not used");
                    return true;
                }
                else
                {
                    loggerService.LogError($"The name is in use");
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggerService.LogError($"The name is in use {ex.Message}");
                return false;
            }
        }
    }
}
