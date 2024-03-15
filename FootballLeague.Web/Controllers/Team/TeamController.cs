namespace FootballLeague.Controllers.Team
{
    using FootballLeague.Core.Interfaces.Team;
    using FootballLeague.Infrastructure.Models.InputModels.Team;
    using FootballLeague.Infrastructure.Models.OutputModels.Team;
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

        [HttpPost("create/")]
        public async Task<ActionResult> CreateTeam(CreateTeamModel model)
        {
            await teamService.CreateAsync(model);
            return Ok();
        }

        [HttpGet("allTeams")]
        public async Task<ActionResult<IEnumerable<AllTeamsOutputModel>>> AllTeams()
        {
            var allTeams = await teamService.GetAllTeamsAsync();
            return allTeams.ToList();
        }

        [HttpGet("teamById")]
        public async Task<ActionResult<TeamByIdOutputModel>> TeamByid(int teamId)
        {
            var team = await teamService.GetTeamByIdAsync(teamId);
            return team;
        }

        [HttpPut("edit")]
        public async Task EditTeam(EditTeamInputModel model, int teamId)
        {
           await teamService.EditTeamAsync(model, teamId);
        }

        [HttpDelete("delete")]
        public async Task DeleteTeam(int teamId)
        {
            await teamService.DeleteTeamAsync(teamId);
        }

        [HttpGet("teamRanking")]
        public async Task<ActionResult<IEnumerable>> TeamRanking()
        {
           var teamsRanking = await teamService.GetAllTeamsRankingAsync();
            return teamsRanking.ToList();
        }
    }
}
