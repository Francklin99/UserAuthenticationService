using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
