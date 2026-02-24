using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/contenidos");
            List<string> rutas = Directory.GetFiles(carpeta)
                                      .Select(f => "/images/contenidos/" + Path.GetFileName(f))
                                      .ToList();

            return View(rutas);
        }
    }
}
