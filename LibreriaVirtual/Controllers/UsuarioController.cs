using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibreriaVirtual.Controllers
{
    public class UsuarioController : Controller
    {
        private IRepositoryLibreria repo;
        private HelperPathProvider helper;

        public UsuarioController(IRepositoryLibreria repo, HelperPathProvider helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        public async Task<IActionResult> Perfil()
        {
            EstadisticasUsuario estadisticas = await repo.GetEstadisticasUsuarioAsync((int)HttpContext.Session.GetInt32("idUsuario"));
            ViewData["estadisticas"] = estadisticas;

            Usuario usuario = await repo.FindUsuarioIdAsync((int)HttpContext.Session.GetInt32("idUsuario"));
            return View(usuario);
        }

        public async Task<IActionResult> Editar()
        {
            Usuario usuario = await repo.FindUsuarioIdAsync((int)HttpContext.Session.GetInt32("idUsuario"));
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(string nombre, IFormFile fichero, string email)
        {
            string imagen = await ComprobarImagenAsync(fichero);

            await repo.UpdateUsuarioAsync(nombre, imagen, email);

            return RedirectToAction("Perfil");
        }

        private async Task<string> ComprobarImagenAsync(IFormFile fichero)
        {
            string imagen;
            if (fichero != null)
            {
                imagen = await SubirFileAsync(fichero);
            }
            else
            {
                Usuario usuario = await repo.FindUsuarioIdAsync((int)HttpContext.Session.GetInt32("idUsuario"));
                imagen = usuario.Imagen;
            }
            return imagen;
        }

        private async Task<string> SubirFileAsync(IFormFile fichero)
        {
            string file = fichero.FileName;
            string ruta = helper.MapPath(file, Carpetas.Users);
            string url = helper.MapUrlPath(file, Carpetas.Users);

            using (Stream stream = new FileStream(ruta, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }

            return url;
        }
    }
}
