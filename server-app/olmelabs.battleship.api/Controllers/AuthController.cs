using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Services;

namespace olmelabs.battleship.api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAuthService _authSvc;

        public AuthController(IConfiguration config, IAuthService authSvc, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _authSvc = authSvc;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModelDto dto)
        {
            IActionResult response = Unauthorized();
            var user = await _authSvc.LoginAsync(dto.Email, dto.Password);
            if (user == null)
                return response;

            var userDto = _mapper.Map<UserModelDto>(user);
            userDto.Token = _authSvc.BuildJwtToken(user); 
            userDto.RefreshToken = _authSvc.BuildRefreshToken(); 

            await _authSvc.AddRefreshTokenAsync(userDto.RefreshToken, user.Email);

            response = Ok(userDto);

            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModelDto dto)
        {
            IActionResult response = Unauthorized();

            if (string.IsNullOrWhiteSpace(dto.Token) || string.IsNullOrWhiteSpace(dto.RefreshToken))
                return response;

            var emailFormRefreshToken = await _authSvc.GetEmailByRefreshTokenAsync(dto.RefreshToken);
            if (string.IsNullOrWhiteSpace(emailFormRefreshToken))
                return response;

            await _authSvc.DeleteRefreshTokenAsync(dto.RefreshToken);

            var principal = _authSvc.GetPrincipalFromExpiredToken(dto.Token);
            var emailFormJwtToken = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (string.IsNullOrWhiteSpace(emailFormJwtToken))
                return response;

            if (emailFormJwtToken != emailFormRefreshToken)
                return response;

            var user = await _authSvc.FindUserAsync(emailFormJwtToken);
            if (user == null)
                return response;

            var userDto = _mapper.Map<UserModelDto>(user);
   
            userDto.Token = _authSvc.BuildJwtToken(user);
            userDto.RefreshToken = _authSvc.BuildRefreshToken();

            await _authSvc.AddRefreshTokenAsync(userDto.RefreshToken, userDto.Email);

            response = Ok(userDto);

            return response;
        }

    }
}