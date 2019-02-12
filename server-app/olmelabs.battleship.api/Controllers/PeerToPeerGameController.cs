using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;
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
        public async Task<IActionResult> JoinSession([FromBody] P2PGameKeyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            PeerToPeerGameState g = await _p2pSvc.JoinSessionAsync(dto.Code, dto.ConnectionId);
            if (g == null)
              return BadRequest();

            if (dto.ConnectionId == g.HostConnectionId)
                return BadRequest();

            await _gameHubContext.Clients.Client(g.HostConnectionId).SendAsync("FriendConnected");

            return Ok(new { });
        }

        [HttpPost]
        [ActionName("StartNewGame")]
        public async Task<IActionResult> StartNewGame([FromBody]P2PGameKeyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest();

            PeerToPeerGameState g = await _p2pSvc.AddPeerToGame(dto.Code, dto.ConnectionId);

            if (g == null)
                return BadRequest();

            if (g.GameStartedCount == 2)
            {
                //Start game here. Implement first move to signalr peer
                var connectionId = dto.ConnectionId == g.HostConnectionId ? g.FriendConnectionId : g.HostConnectionId;
                await _gameHubContext.Clients.Client(connectionId).SendAsync("GameStarted");
            }
            else {

                var connectionId = dto.ConnectionId == g.HostConnectionId ? g.FriendConnectionId : g.HostConnectionId;
                await _gameHubContext.Clients.Client(connectionId).SendAsync("FriendStartedGame");
            }

            return Ok(new { });
        }
    }
}
