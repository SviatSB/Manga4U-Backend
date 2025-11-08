using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class MyController : ControllerBase
    {
        protected string? ContextLogin { get => User.Identity?.Name; }
    }
}
