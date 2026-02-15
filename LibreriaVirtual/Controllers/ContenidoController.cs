using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class ContenidoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
