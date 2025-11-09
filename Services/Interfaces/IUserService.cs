using System.Threading.Tasks;

using Services.DTOs.ModelsDTOs;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> FindAsync(string login);
        Task<UserDto?> FindAsync(long userId);
    }
}
