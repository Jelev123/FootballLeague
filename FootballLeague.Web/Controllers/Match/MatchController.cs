namespace FootballLeague.Controllers.Match
{
    using FootballLeague.Core.Contracts.Match;
    using FootballLeague.Infrastructure.InputModels.Match;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
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
            await matchService.CreateMatchAsync(model);
            return Ok();
        }

        [HttpGet("AllMatches")]
        public async Task<ActionResult<IEnumerable<AllMatchesInputModels>>> AllMatches()
        {
            var allMatches = await matchService.AllMatchesAsync();
            return allMatches.ToList();
        }

        [HttpGet("MatchById")]
        public async Task<ActionResult<MatchByIdInputModel>> MatchById(int matchId)
        {
            var match = await matchService.GetmatchByIdAsync(matchId);
            return match;
        }

        [HttpPut("Edit")]
        public async Task EditMatch(EditMatchInputModel model, int matchId)
        {
           await matchService.EditMatchAsync(model, matchId);
        }

        [HttpDelete("Delete")]
        public async Task DeleteMatch(int matchId)
        {
            await matchService.DeleteMatchAsync(matchId);
        }
    }
}
