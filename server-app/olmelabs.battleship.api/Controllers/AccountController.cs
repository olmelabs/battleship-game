using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services;

namespace olmelabs.battleship.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountSvc;

        public AccountController(IConfiguration config, IAccountService accountSvc, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _accountSvc = accountSvc;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDto dto)
        {
            var user = await _accountSvc.FindUserAsync(dto.Email);

            RegisterResponseDto respModel = new RegisterResponseDto();
            if (user != null)
            {
                respModel.Success = false;
                respModel.Message = "This email is already taken";
                return Ok(respModel);
            }

            user = _mapper.Map<User>(dto);

            var newUser = _accountSvc.RegisterUserAsync(user);

            respModel.Success = true;
            return Ok(respModel);
        }
    }
}