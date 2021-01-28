using AuthServer.Core.DTOs;
using SharedLibrary.DTO;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<Response<AppUserDTO>> CreateUserAsync(CreateUserDTO user);
        Task<Response<AppUserDTO>> GetUserByNameAsync(string userName);
    }
}
