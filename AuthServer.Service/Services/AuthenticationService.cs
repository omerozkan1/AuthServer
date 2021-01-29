using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AuthServer.Core.Repository;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

        public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = clients.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }

        public async Task<Response<TokenDTO>> CreateTokenAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null) throw new ArgumentNullException(nameof(loginDTO));

            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null) return Response<TokenDTO>.Fail("Email veya Parola yanlış", (int)HttpStatusCode.BadRequest, true);

            if(!await _userManager.CheckPasswordAsync(user,loginDTO.Password))
                return Response<TokenDTO>.Fail("Email veya Parola yanlış", (int)HttpStatusCode.BadRequest, true);

            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();
            return Response<TokenDTO>.Success(token, (int)HttpStatusCode.OK);
        }

        public Response<ClientTokenDTO> CreateTokenByClient(ClientLoginDTO clientLoginDTO)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDTO.ClientId && x.Secret == clientLoginDTO.ClientSecret);
            if (client == null)
                return Response<ClientTokenDTO>.Fail("ClientId veya ClientSecret bulunamadı.", (int)HttpStatusCode.NotFound, true);

            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDTO>.Success(token, (int)HttpStatusCode.OK);
        }

        public async Task<Response<TokenDTO>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
                return Response<TokenDTO>.Fail("Refresh token bulunamadı.", (int)HttpStatusCode.NotFound, true);

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
            if (user == null)
                return Response<TokenDTO>.Fail("User Id bulunamadı.", (int)HttpStatusCode.NotFound, true);

            var tokenDto = _tokenService.CreateToken(user);
            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();
            return Response<TokenDTO>.Success(tokenDto, (int)HttpStatusCode.OK);
        }

        public async Task<Response<NoDataDTO>> RevokeRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken == null)
                return Response<NoDataDTO>.Fail("Refresh token bulunamadı.", (int)HttpStatusCode.NotFound, true);

            _userRefreshTokenService.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDTO>.Success((int)HttpStatusCode.OK);
        }
    }
}
