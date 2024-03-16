namespace FootballLeague.Core.Services.Team
{
    using Amazon.CloudWatchLogs.Model;
    using FootballLeague.Core.Constants;
    using FootballLeague.Core.Constants.Logger;
    using FootballLeague.Infrastructure.Constants.Logger.Team;
    using FootballLeague.Core.Contracts.Loger;
    using FootballLeague.Core.Interfaces.Team;
    using FootballLeague.Core.Handlers.ErrorHandlers;
    using FootballLeague.Infrastructure.Data;
    using FootballLeague.Infrastructure.Data.Models;
    using FootballLeague.Infrastructure.Models.InputModels.Team;
    using FootballLeague.Infrastructure.Models.OutputModels.Team;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeamDecoratorService : ITeamService
    {
        private readonly TeamService teamService;
        private readonly ILoggerService loggerService;
        private readonly ApplicationDbContext data;

        public TeamDecoratorService(TeamService teamService, ILoggerService loggerService, ApplicationDbContext data)
        {
            this.teamService = teamService;
            this.loggerService = loggerService;
            this.data = data;
        }

        public async Task CreateAsync(CreateTeamModel model)
        {
            loggerService.LogInfo(TeamRequestType.CreateTeam.ToString());

            var isTheNameExist = await this.IsTheNameAlreadyExistAsync(model.TeamName);

            if (isTheNameExist)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());

                throw new DataAlreadyExistsException(string.Format(
                    ErrorMessages.DataAlreadyExists,
                    typeof(Team).Name, model.TeamName));
            }

            await teamService.CreateAsync(model);

            loggerService.LogInfo(RequestStatus.Success.ToString());
        }

        public async Task DeleteTeamAsync(int teamId)
        {
            loggerService.LogInfo(TeamRequestType.DeleteTeam.ToString());

            var team = await data.Teams
                .Where(team => !team.IsDeleted)
                .FirstOrDefaultAsync(team => team.Id == teamId && !team.IsDeleted);

            if (team == null)
            {
                loggerService.LogInfo(RequestStatus.Failed.ToString());
                throw new ResourceNotFoundException(string.Format(
                    ErrorMessages.DataDoesNotExist,
                    typeof(Team).Name, "id", teamId));
            }

            await teamService.DeleteTeamAsync(teamId);
            loggerService.LogInfo(RequestStatus.Success.ToString());
        }

        public async Task EditTeamAsync(EditTeamInputModel model, int teamId)
        {
            loggerService.LogInfo(TeamRequestType.UpdateTeam.ToString());

            var team = await data.Teams.FirstOrDefaultAsync(t => t.Id == teamId && !t.IsDeleted);

            if (team == null)
            {
                loggerService.LogError(RequestStatus.Failed.ToString());
                throw new ResourceNotFoundException(string.Format(
                   ErrorMessages.DataDoesNotExist,
                   typeof(Team).Name, "id", teamId));
            }
            await teamService.EditTeamAsync(model, teamId);
            loggerService.LogInfo(RequestStatus.Success.ToString());
        }


        public async Task<IEnumerable<AllTeamsOutputModel>> GetAllTeamsAsync()
        {
            loggerService.LogInfo(TeamRequestType.GetAllTeams.ToString());

            var teams = await teamService.GetAllTeamsAsync();

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return teams;
        }

        public async Task<ICollection<TeamRankingOutputModel>> GetAllTeamsRankingAsync()
        {
            loggerService.LogInfo(TeamRequestType.GetAllTeams.ToString());

            var teams = await teamService.GetAllTeamsRankingAsync();

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return teams;
        }

        public async Task<TeamByIdOutputModel> GetTeamByIdAsync(int teamId)
        {
            loggerService.LogInfo(TeamRequestType.GetTeam.ToString());

            var team = await teamService.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                loggerService.LogError(RequestStatus.Failed.ToString());
                throw new ResourceNotFoundException(string.Format(
                   ErrorMessages.DataDoesNotExist,
                   typeof(Team).Name, "id", teamId));
            }

            loggerService.LogInfo(RequestStatus.Success.ToString());

            return team;
        }

        public async Task<bool> IsTheNameAlreadyExistAsync(string teamName)
        {
            return await data.Teams.AnyAsync(t => t.Name == teamName && !t.IsDeleted);
        }
    }
}
