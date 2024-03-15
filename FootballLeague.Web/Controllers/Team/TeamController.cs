namespace FootballLeague.Controllers.Team
{
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Infrastructure.InputModels.Team;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpPost("Create/")]
        public async Task<ActionResult> CreateTeam(CreateTeamInputModel model)
        {
            await teamService.CreateAsync(model);
            return Ok();
        }

        [HttpGet("AllTeams")]
        public async Task<ActionResult<IEnumerable<AllTeamsModel>>> AllTeams()
        {
            var allTeams = await teamService.GetAllTeamsAsync();
            return allTeams.ToList();
        }

        [HttpGet("TeamById")]
        public async Task<ActionResult<TeamByIdInputModel>> TeamByid(int teamId)
        {
            var team = await teamService.GetTeamById(teamId);
            return team;
        }

        [HttpPut("Edit")]
        public async Task EditTeam(EditTeamInputModel model, int teamId)
        {
           await teamService.EditTeamAsync(model, teamId);
        }

        [HttpDelete("Delete")]
        public async Task DeleteTeam(int teamId)
        {
            await teamService.DeleteTeamAsync(teamId);
        }

        [HttpGet("TeamRanking")]
        public async Task<ActionResult<IEnumerable>> TeamRanking()
        {
           var teamsRanking = await teamService.GetAllTeamsRankingAsync();
            return teamsRanking.ToList();
        }
    }
}
