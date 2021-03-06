﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;

namespace olmelabs.battleship.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly IGameService _gameSvc;
        private readonly IMapper _mapper;
        private readonly IGameStatisticsService _statisticsSvc;

        public GameController(IGameService gameSvc, IMapper mapper, IGameStatisticsService statisticsSvc)
        {
            _gameSvc = gameSvc;
            _mapper = mapper;
            _statisticsSvc = statisticsSvc;
        }

        [HttpPost]
        [ActionName("ValidateBoard")]
        public IActionResult ValidateBoard([FromBody]ClientShipDto[] dto)
        {
            bool res = false;
            if (dto != null)
            {
                res = _gameSvc.ValidateClientBoard(dto.ToList()); 
            }

            return Ok(new { result = res });
        }

        [HttpPost]
        [ActionName("GenerateBoard")]
        public IActionResult GenerateBoard()
        {
            BoardInfo board = _gameSvc.GenerateClientBoard();
            BoardInfoDto respDto = _mapper.Map<BoardInfoDto>(board);

            return Ok(respDto);
        }

        [HttpPost]
        [ActionName("StartNewGame")]
        public async Task<IActionResult> StartNewGame([FromBody]string connectionId)
        {
            GameState game = await _gameSvc.StartNewGameAsync(connectionId);
            NewGameDto respDto = _mapper.Map<NewGameDto>(game);

            return Ok(respDto);
        }

        [HttpPost]
        [ActionName("StopGame")]
        public async Task<IActionResult> StopGame([FromBody]GameOverClientDto dto)
        {
            List <ShipInfo> clientShips = _mapper.Map<List<ShipInfo>>(dto.Ships);
            var game = await _gameSvc.StopGameAsync(dto.GameId, clientShips);

            if (game == null)
                return BadRequest();

            _statisticsSvc.EnqueueGameStatistics(clientShips);

            GameOverDto respDto = _mapper.Map<GameOverDto>(game);

            return Ok(respDto);
        }

        [HttpPost]
        [ActionName("FireCannon")]
        //Called by client when its move
        public async Task<IActionResult> FireCannon([FromBody]FireCannonDto dto)
        {
            FireResult res = await _gameSvc.FireCannon(dto.GameId, dto.CellId);

            if (res == null)
                return BadRequest();

            FireCannonResultDto respDto = _mapper.Map<FireCannonResultDto>(res);
            respDto.CellId = dto.CellId;
            respDto.GameId = dto.GameId;

            return Ok(respDto);
        }

        [HttpPost]
        [ActionName("FireCannonProcessResult")]
        //Called by client in response to server move (signal R)
        public async Task<IActionResult> FireCannonProcessResult([FromBody]FireCannonCallbackDto dto)
        {
            var game = await _gameSvc.FireCannonProcessResult(dto);

            if (game == null)
                return BadRequest();

            return Ok(new { });
        }
    }
}
