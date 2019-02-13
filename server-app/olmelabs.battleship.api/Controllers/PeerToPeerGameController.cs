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
    public class PeerToPeerGameController: Controller
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

            return Ok(new { code = g.Code });
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
        public async Task<IActionResult> StartNewGame([FromBody]P2PGameKeyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionId))
                return BadRequest();

            if (string.IsNullOrWhiteSpace(dto.Code))
                return BadRequest();

            PeerToPeerSessionState session = await _p2pSvc.AddPeerToSession(dto.Code, dto.ConnectionId);

            if (session == null)
                return BadRequest();

            if (session.GameStartedCount == 2)
            {

                PeerToPeerGameState game = await _p2pSvc.StartNewGameAsync(session);
                NewGameDto respDto = _mapper.Map<NewGameDto>(game);

                var connectionId = dto.ConnectionId == session.HostConnectionId ? session.FriendConnectionId : session.HostConnectionId;
                await _gameHubContext.Clients.Client(connectionId).SendAsync("GameStartedYourMove", respDto);
                await _gameHubContext.Clients.Client(dto.ConnectionId).SendAsync("GameStartedFriendsMove", respDto);

                return Ok(respDto);
            }
            else {

                var connectionId = dto.ConnectionId == session.HostConnectionId ? session.FriendConnectionId : session.HostConnectionId;
                await _gameHubContext.Clients.Client(connectionId).SendAsync("FriendStartedGame");
                await _gameHubContext.Clients.Client(dto.ConnectionId).SendAsync("YouStartedGame");
            }

            return Ok(new { });
        }
    }
}
