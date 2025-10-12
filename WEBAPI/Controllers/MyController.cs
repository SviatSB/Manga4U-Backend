using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Controllers
{
    public class MyController : ControllerBase
    {
        protected string? ContextLogin { get => User.Identity?.Name; }
    }
}
