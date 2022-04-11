using Microsoft.AspNetCore.Mvc;

namespace ClientServer.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
