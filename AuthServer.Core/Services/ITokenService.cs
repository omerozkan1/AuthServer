using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDTO CreateToken(AppUser appUser);
        ClientTokenDTO CreateTokenByClient(Client client);
    }
}
