using Services.Interfaces;

namespace WebApi.Controllers
{
    public class StatController(IUserService userService) : MyController(userService)
    {
        
    }
}
