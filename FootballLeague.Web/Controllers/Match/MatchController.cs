namespace FootballLeague.Controllers.Match
{
    using FootballLeague.Core.Interfaces.Match;
    using FootballLeague.Infrastructure.Models.InputModels.Match;
    using FootballLeague.Infrastructure.Models.OutputModels.Match;
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

        [HttpPost("create/")]
        public async Task<ActionResult> CreateMatch(CreateMatchModel model)
        {
            await matchService.CreateMatchAsync(model);
            return Ok();
        }

        [HttpGet("allMatches")]
        public async Task<ActionResult<IEnumerable<MatchOutputModels>>> AllMatches()
        {
            var allMatches = await matchService.AllMatchesAsync();
            return allMatches.ToList();
        }

        [HttpGet("matchById")]
        public async Task<ActionResult<MatchOutputModels>> MatchById(int matchId)
        {
            var match = await matchService.GetmatchByIdAsync(matchId);
            return match;
        }

        [HttpPut("edit")]
        public async Task EditMatch(EditMatchModel model, int matchId)
        {
           await matchService.EditMatchAsync(model, matchId);
        }

        [HttpDelete("delete")]
        public async Task DeleteMatch(int matchId)
        {
            await matchService.DeleteMatchAsync(matchId);
        }
    }
}
