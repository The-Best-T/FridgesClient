using Microsoft.AspNetCore.Mvc;

namespace ClientServer.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(model:HttpContext.Request.Cookies["JWT"]);
        }
    }
}
