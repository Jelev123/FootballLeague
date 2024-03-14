namespace FootballLeague.Controllers.Match
{
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Infrastructure.InputModels.Match;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService matchService;

        public MatchController(IMatchService matchService)
        {
            this.matchService = matchService;
        }

        [HttpPost("Create/")]
        public async Task<IActionResult> CreateTeam(CreateMatchInputModel model)
        {
            try
            {
                await matchService.CreateMatchAsync(model);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Failed to create match: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to create match: " + ex.Message);
            }
        }

        [HttpGet("AllMatches")]
        public async Task<IActionResult> AllMatches()
        {
            try
            {
                return Ok(await matchService.AllMatchesAsync());
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Failed to get all matches: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to get all matches: " + ex.Message);
            }
        }

        [HttpGet("MatchById")]
        public async Task<IActionResult> MatchById(int matchId)
        {
            try
            {
                return Ok(await matchService.GetmatchByIdAsync(matchId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Failed to get match: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to get match: " + ex.Message);
            }
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditMatch(EditMatchInputModel model, int matchId)
        {
            try
            {
                return Ok(await matchService.EditMatchAsync(model, matchId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Failed to edit match: " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to edit match: " + ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteMatch(int matchId)
        {
            try
            {
                bool result = await matchService.DeleteMatchAsync(matchId);
                if (result)
                {
                    return Ok("Match edited successfully.");
                }
                else
                {
                    return NotFound("Match not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to delte match: " + ex.Message);
            }
        }
    }
}
