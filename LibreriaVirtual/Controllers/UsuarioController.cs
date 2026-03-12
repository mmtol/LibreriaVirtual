using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            int? idUsuario = HttpContext.Session.GetInt32("idUsuario");

            if (idUsuario != null)
            {
                EstadisticasUsuario estadisticas = await repo.GetEstadisticasUsuarioAsync((int)idUsuario);
                ViewData["estadisticas"] = estadisticas;

                List<Contenido> favs = await repo.GetContenidosFavoritosAsync((int)idUsuario);
                ViewData["favs"] = favs;

                Usuario usuario = await repo.FindUsuarioIdAsync((int)idUsuario);
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> Editar()
        {
            int? idUsuario = HttpContext.Session.GetInt32("idUsuario");

            if (idUsuario != null)
            {
                Usuario usuario = await repo.FindUsuarioIdAsync((int)idUsuario);
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(string nombre, IFormFile fichero, string email, string pass)
        {
            int? idUsuario = HttpContext.Session.GetInt32("idUsuario");

            if (idUsuario != null)
            {
                string imagen = await ComprobarImagenAsync(fichero);

                await repo.UpdateUsuarioAsync((int)HttpContext.Session.GetInt32("idUsuario"), nombre, imagen, email);

                if (pass != null)
                {
                    await repo.UpdatePassAsync((int)HttpContext.Session.GetInt32("idUsuario"), pass);
                }

                return RedirectToAction("Perfil");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        private async Task<string> ComprobarImagenAsync(IFormFile fichero)
        {
            string imagen;
            if (fichero != null && fichero.Length > 0)
            {
                imagen = await SubirFileAsync(fichero);
                HttpContext.Session.Remove("imgUsuario");
                HttpContext.Session.SetString("imgUsuario", imagen);
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
