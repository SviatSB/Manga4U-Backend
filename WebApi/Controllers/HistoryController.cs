using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : MyController
    {
        //получить историю (страницами с офсетами и пагинацией)
        //обновить историю (пост манг + язык + глава)
        //отдать список жанров с кофициентами
    }
}
