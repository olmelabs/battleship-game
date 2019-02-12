using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using olmelabs.battleship.api.Models.Dto;
using olmelabs.battleship.api.Models.Entities;
using olmelabs.battleship.api.Services.Interfaces;

namespace olmelabs.battleship.api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountSvc;
        private readonly INotificationService _notificationService;

        public AccountController(IConfiguration config, IAccountService accountSvc, IMapper mapper, INotificationService notificationService)
        {
            _config = config;
            _mapper = mapper;
            _accountSvc = accountSvc;
            _notificationService = notificationService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDto dto)
        {
            var user = await _accountSvc.FindUserAsync(dto.Email);

            SimpleResponseDto respDto = new SimpleResponseDto();
            if (user != null)
            {
                respDto.Success = false;
                respDto.Message = "This email is already taken.";
                return Ok(respDto);
            }

            user = _mapper.Map<User>(dto);

            string code = await _accountSvc.RegisterEmailConfirmationCodeAsync(user);
            await _notificationService.SendConfirmEmailMailAsync(code, user);

            var newUser = _accountSvc.RegisterUserAsync(user);

            respDto.Success = true;
            return Ok(respDto);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] string code)
        {
            SimpleResponseDto respDto = new SimpleResponseDto();

            User user = await _accountSvc.GetUserByEmailConfirmationCodeAsync(code);

            if (user == null || user.IsEmailConfirmed)
            {
                respDto.Success = false;
                respDto.Message = "Email confirmation failed. Possibly email is already confirmed.";
                return Ok(respDto);
            }

            await _accountSvc.ConfirmEmailAsync(user);

            respDto.Success = true;
            return Ok(respDto);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("SendResetPasswordLink")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody] string email)
        {
            var user = await _accountSvc.FindUserAsync(email);

            if (user != null && user.IsEmailConfirmed)
            {
                string code = await _accountSvc.RegisterResetPasswordCodeAsync(user);
                await _notificationService.SendResetPasswordMailAsync(code, user);
            }

            //always return Ok- do not tell the world if email exists or not.
            return Ok(new SimpleResponseDto { Success = true });
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            SimpleResponseDto respDto = new SimpleResponseDto();

            User user = await _accountSvc.GetUserByResetPasswordTokenAsync(dto.Code);

            if (user == null || string.IsNullOrWhiteSpace(dto.Password) || dto.Password != dto.Password2)
            {
                respDto.Success = false;
                respDto.Message = "Reset password fail.";
                return Ok(respDto);
            }

            user.Password = dto.Password;
            await _accountSvc.ResetPasswordAsync(user);

            respDto.Success = true;

            return Ok(respDto);
        }

    }
}