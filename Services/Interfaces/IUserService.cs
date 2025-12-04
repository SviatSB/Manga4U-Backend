using System.Threading.Tasks;

using Services.DTOs.AccountDTOs;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> FindAsync(string login);
        Task<UserDto?> FindAsync(long userId);
    }
}
