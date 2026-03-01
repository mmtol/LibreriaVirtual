using LibreriaVirtual.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class ContenidoController : Controller
    {
        public IActionResult Index(bool personal)
        {
            ViewData["personal"] = personal;
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Recomendaciones()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string tipo, string genero, bool personal)
        {
            ViewData["personal"] = personal;
            return View();
        }

        [HttpPost]
        public IActionResult Ver(int idcontenido, int puntuacion, string opinion)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int idcontenido)
        {
            return View();
        }

        [HttpPost]
        public IActionResult UpdateOpinion(int idcontenido, int puntuacion, string opinion)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Apropiar(int idcontenido)
        {
            return View();
        }
    }
}
