using AutoMapper;
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
    public class PeerToPeerGameController : Controller
    {
        private readonly IPeerToPeerGameService _p2pSvc;
        private readonly IMapper _mapper;
        private readonly IHubContext<GameHub> _gameHubContext;

        public PeerToPeerGameController(
            IPeerToPeerGameService p2pSvc,
            IMapper mapper,
            IHubContext<GameHub> gameHubContext
            )
        {
            _p2pSvc = p2pSvc;
            _mapper = mapper;
            _gameHubContext = gameHubContext;
        }

        [HttpPost]
        [ActionName("StartSession")]
        public async Task<IActionResult> StartSession([FromBody] string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                return BadRequest();

            PeerToPeerSessionState g = await _p2pSvc.StartNewSessionAsync(connectionId);

            return Ok(new P2PStartSessionDto { Code = g.Code });
        }


        [HttpPost]
        [ActionName("JoinSession")]
        public async Task<IActionResult> JoinSession([FromBody] P2PGameKeyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            PeerToPeerSessionState g = await _p2pSvc.JoinSessionAsync(dto.Code, dto.ConnectionId);
            if (g == null)
                return BadRequest();

            if (dto.ConnectionId == g.HostConnectionId)
                return BadRequest();

            await _gameHubContext.Clients.Client(g.HostConnectionId).SendAsync("FriendConnected");

            return Ok(new { });
        }

        [HttpPost]
        [ActionName("StartNewGame")]
        public async Task<IActionResult> StartNewGame([FromBody]P2PNewGametDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest();

            if (dto.Ships == null || dto.Ships.Length != 10)
                return BadRequest();

            PeerToPeerSessionState session = await _p2pSvc.AddPeerToSession(dto.Code, dto.ConnectionId);

            if (session == null)
                return BadRequest();

            if (session.GameStartedCount == 2)
            {
                //TODO: Implement add board to Game
                session = await _p2pSvc.StartNewGameAsync(session);

                var connectionId = dto.ConnectionId == session.HostConnectionId ? session.FriendConnectionId : session.HostConnectionId;
                await _gameHubContext.Clients.Client(connectionId).SendAsync("GameStartedYourMove", new P2PNewGameResultDto { GameId = session.GameId, YourMove = true });
                await _gameHubContext.Clients.Client(dto.ConnectionId).SendAsync("GameStartedFriendsMove", new P2PNewGameResultDto { GameId = session.GameId, YourMove = false });
            }
            else
            {

                var connectionId = dto.ConnectionId == session.HostConnectionId ? session.FriendConnectionId : session.HostConnectionId;
                await _gameHubContext.Clients.Client(connectionId).SendAsync("FriendStartedGame");
                await _gameHubContext.Clients.Client(dto.ConnectionId).SendAsync("YouStartedGame");
            }

            return Ok(new { });
        }

        [HttpPost]
        [ActionName("FireCannon")]
        public async Task<IActionResult> FireCannon([FromBody]P2PFireCannonDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest();

            PeerToPeerSessionState session = await _p2pSvc.FindActiveSessionAsync(dto.Code, dto.ConnectionId);

            if (session == null)
                return BadRequest();

            var connectionId = dto.ConnectionId == session.HostConnectionId ? session.FriendConnectionId : session.HostConnectionId;

            FireCannonDto srDto = new FireCannonDto
            {
                GameId = session.GameId,
                CellId = dto.CellId
            };

            await _gameHubContext.Clients.Client(connectionId).SendAsync("MakeFireFromServer", srDto);

            return Ok(new { });
        }

        [HttpPost]
        [ActionName("FireCannonProcessResult")]
        public async Task<IActionResult> FireCannonProcessResult([FromBody]P2PFireCannonCallbackDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest();

            PeerToPeerSessionState session = await _p2pSvc.FindActiveSessionAsync(dto.Code, dto.ConnectionId);

            if (session == null)
                return BadRequest();

            FireCannonResultDto respDto = new FireCannonResultDto
            {
                CellId = dto.CellId,
                ShipDestroyed = dto.ShipDestroyed,
                IsAwaitingServerTurn = !dto.Result,
                IsGameOver = dto.IsGameOver,
                Result = dto.Result
            };

            var connectionId = dto.ConnectionId == session.HostConnectionId ? session.FriendConnectionId : session.HostConnectionId;

            await _gameHubContext.Clients.Client(connectionId).SendAsync("MakeFireProcessResult", respDto);

            return Ok(new { });
        }
    }
}
