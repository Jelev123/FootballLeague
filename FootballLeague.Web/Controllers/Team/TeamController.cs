namespace FootballLeague.Controllers.Team
{
    using FootballLeague.Core.Contracts.Team;
    using FootballLeague.Infrastructure.InputModels.Team;
    using Microsoft.AspNetCore.Mvc;
    using System;
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
        public async Task<IActionResult> CreateTeam(CreateTeamInputModel model)
        {
            try
            {
                await teamService.CreateAsync(model);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Failed to create team: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to create team: " + ex.Message);
            }
        }

        [HttpGet("AllTeams")]
        public async Task<IActionResult> AllTeams()
        {
            try
            {
                return Ok(await teamService.GetAllTeamsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get all teams: " + ex.Message);
            }
        }

        [HttpGet("TeamById")]
        public async Task<IActionResult> TeamByid(int teamId)
        {
            try
            {
                return Ok(await teamService.GetTeamById(teamId));
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get  team: " + ex.Message);
            }
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditTeam(EditTeamInputModel model, int teamId)
        {
            try
            {
                bool result = await teamService.EditTeamAsync(model, teamId);
                if (result)
                {
                    return Ok("Team edited successfully.");
                }
                else
                {
                    return NotFound("Team not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to edit team: " + ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTeam( int teamId)
        {
            try
            {
                bool result = await teamService.DeleteTeamAsync(teamId);
                if (result)
                {
                    return Ok("Team delete successfully.");
                }
                else
                {
                    return NotFound("Team not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to delete team: " + ex.Message);
            }
        }

        [HttpGet("TeamRanking")]
        public async Task<IActionResult> TeamRanking()
        {
            try
            {
                return Ok(await teamService.GetAllTeamsRankingAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to get  team ranking: " + ex.Message);
            }
        }
    }
}
