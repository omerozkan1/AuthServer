using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDTO login)
        {
            var result = await _authenticationService.CreateTokenAsync(login);
            return ActionResultInstance(result);           
        }

        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDTO client)
        {
            var result = _authenticationService.CreateTokenByClient(client);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDTO refreshToken)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshToken.Token);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDTO refreshToken)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshToken.Token);
            return ActionResultInstance(result);
        }
    }
}
