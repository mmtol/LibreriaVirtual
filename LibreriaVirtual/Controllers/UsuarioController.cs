using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
