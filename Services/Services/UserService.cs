using DataInfrastructure.Interfaces;

using Services.DTOs.ModelsDTOs;
using Services.Interfaces;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto?> FindAsync(string login)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null) return null;
            var roles = await _userRepository.GetRolesAsync(user);
            return DtoConvertor.CreateUserDto(user, roles);
        }

        public async Task<UserDto?> FindAsync(long userId)
        {
            var user = await _userRepository.FindAsync(userId);
            if (user == null) return null;
            var roles = await _userRepository.GetRolesAsync(user);
            return DtoConvertor.CreateUserDto(user, roles);
        }
    }
}
