using LibreriaVirtual.Filter;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeUsuariosAttribute]
        public IActionResult Index()
        {
            return View();
        }
    }
}
