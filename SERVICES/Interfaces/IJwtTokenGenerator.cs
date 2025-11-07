using Domain.Models;

namespace Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(User user, IList<string> roles);
    }
}
