using AutoMapper;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using System.Linq;

namespace olmelabs.battleship.api.Models
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<GameState, NewGameDto>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId));

            CreateMap<PeerToPeerGameState, NewGameDto>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.GameId));
            
            CreateMap<GameState, GameOverDto>()
                .ForMember(dest => dest.Ships, opt => opt.MapFrom(src => src.ServerBoard.Ships.Select(s => s.Cells).ToList()));

            CreateMap<FireResult, FireCannonResultDto>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.IsHit))
                .ForMember(dest => dest.ShipDestroyed, opt => opt.MapFrom(src => src.ShipDestroyed == null ? null : src.ShipDestroyed.Cells))
                .ForMember(dest => dest.IsGameOver, opt => opt.MapFrom(src => src.IsGameOver))
                .ForMember(dest => dest.IsAwaitingServerTurn, opt => opt.MapFrom(src => !src.IsHit));

            CreateMap<User, UserModelDto>();

            CreateMap<RegisterModelDto, User>()
                 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()));

            CreateMap<ClientShipDto, ShipInfo>();
            CreateMap<ShipInfo, ClientShipDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => $"{src.Cells.Length}X"));

            CreateMap<BoardInfo, BoardInfoDto>();
        }
    }
}
