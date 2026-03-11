using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
