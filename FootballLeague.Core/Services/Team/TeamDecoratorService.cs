namespace FootballLeague.Core.Services.Team
{
    using FootballLeague.Core.Constants;
    using FootballLeague.Core.Constants.Logger;
    using FootballLeague.Core.Contracts.Loger;
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Core.Handlers.ErrorHandlers;
    using FootballLeague.Infrastructure.Data.Models;
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
            loggerService.LogInfo(RequestType.CreateTeam.ToString());

            bool isTheNameAlreadyExist = await this.IsTheNameAlreadyExist(model.TeamName);

            if (!isTheNameAlreadyExist)
            {
                await teamService.CreateAsync(model);

                loggerService.LogInfo(RequestStatus.Success.ToString());
            }
            else
            {
                loggerService.LogError(RequestStatus.Failed.ToString());
            }
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            loggerService.LogInfo(RequestType.DeleteTeam.ToString());

            if (await teamService.DeleteTeamAsync(teamId))
            {
                loggerService.LogInfo(RequestStatus.Success.ToString());

                return true;
            }
            else
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                return false;
            }
        }

        public async Task<bool> EditTeamAsync(EditTeamInputModel model, int teamId)
        {
            loggerService.LogInfo(RequestType.UpdateTeam.ToString());

            bool editSuccess = await teamService.EditTeamAsync(model, teamId);

            if (editSuccess)
            {
                loggerService.LogInfo(RequestStatus.Success.ToString());
                return true;
            }
            else
            {
                loggerService.LogError(RequestStatus.Failed.ToString());
                return false;
            }
        }

        public async Task<IEnumerable<AllTeamsModel>> GetAllTeamsAsync()
        {
            loggerService.LogInfo(RequestType.GetAllTeams.ToString());

            var teams = await teamService.GetAllTeamsAsync();

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return teams;
        }

        public async Task<ICollection<AllTeamsRanking>> GetAllTeamsRankingAsync()
        {
            loggerService.LogInfo(RequestType.GetAllTeams.ToString());

            var teams = await teamService.GetAllTeamsRankingAsync();

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return teams;
        }

        public async Task<TeamByIdInputModel> GetTeamById(int teamId)
        {
            loggerService.LogInfo(RequestType.GetTeam.ToString());

            var team = await teamService.GetTeamById(teamId);

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return team;
        }

        public async Task<bool> IsTheNameAlreadyExist(string teamName)
        {
            bool isAlreadyExist = await teamService.IsTheNameAlreadyExist(teamName);

            if (isAlreadyExist)
            {
                throw new ResourceAlreadyExistsException(string.Format(
                ErrorMessages.DataAlreadyExists,
                typeof(Team).Name, teamName));
            }
            return false;
        }
    }
}
