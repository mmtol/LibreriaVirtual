using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
