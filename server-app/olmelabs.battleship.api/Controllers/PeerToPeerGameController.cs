using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services;
using olmelabs.battleship.api.SignalRHubs;
using System.Threading.Tasks;

namespace olmelabs.battleship.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PeerToPeerGameController: Controller
    {
        private readonly IPeerToPeerGameService _p2pSvc;
        private readonly IHubContext<GameHub> _gameHubContext;

        public PeerToPeerGameController(
            IPeerToPeerGameService p2pSvc,
            IHubContext<GameHub> gameHubContext
            )
        {
            _p2pSvc = p2pSvc;
            _gameHubContext = gameHubContext;
        }

        [HttpPost]
        [ActionName("StartSession")]
        public async Task<IActionResult> StartSession([FromBody] string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                return BadRequest();

            PeerToPeerGameState g = await _p2pSvc.StartNewSessionAsync(connectionId);

            return Ok(new { code = g.Code });
        }


        [HttpPost]
        [ActionName("JoinSession")]
        public async Task<IActionResult> JoinSession([FromBody] JoinSessionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            PeerToPeerGameState g = await _p2pSvc.JoinSessionAsync(dto.Code, dto.ConnectionId);
            if (g == null)
              return NotFound();

            if (dto.ConnectionId == g.HostConnectionId)
                return BadRequest();

            await _gameHubContext.Clients.Client(g.HostConnectionId).SendAsync("FriendConnected", dto.ConnectionId);

            return Ok(new { });
        }
    }
}
