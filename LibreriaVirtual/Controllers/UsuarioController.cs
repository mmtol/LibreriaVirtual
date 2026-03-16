using LibreriaVirtual.Filter;
using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Perfil(string id)
        {
            EstadisticasUsuario estadisticas = await repo.GetEstadisticasUsuarioAsync(int.Parse(id));
            ViewData["estadisticas"] = estadisticas;

            List<Contenido> favs = await repo.GetContenidosFavoritosAsync(int.Parse(id));
            ViewData["favs"] = favs;

            Usuario usuario = await repo.FindUsuarioIdAsync(int.Parse(id));
            return View(usuario);
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Editar(string id)
        {
            Usuario usuario = await repo.FindUsuarioIdAsync(int.Parse(id));
            return View(usuario);
        }

        [AuthorizeUsuariosAttribute]
        [HttpPost]
        public async Task<IActionResult> Editar(string nombre, IFormFile fichero, string email, string pass, string id)
        {
            string imagen = await ComprobarImagenAsync(fichero, id);

            await repo.UpdateUsuarioAsync(int.Parse(id), nombre, imagen, email);

            if (pass != null)
            {
                await repo.UpdatePassAsync(int.Parse(id), pass);
            }

            Usuario usuario = await repo.FindUsuarioIdAsync(int.Parse(id));
            await RefrescarClaimsUsuario(usuario);

            return RedirectToAction("Perfil", new { id = id });
        }

        private async Task<string> ComprobarImagenAsync(IFormFile fichero, string id)
        {
            string imagen;
            if (fichero != null && fichero.Length > 0)
            {
                imagen = await SubirFileAsync(fichero);
            }
            else
            {
                Usuario usuario = await repo.FindUsuarioIdAsync(int.Parse(id));
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

        private async Task RefrescarClaimsUsuario(Usuario usuario)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name,
                ClaimTypes.Role
            );

            identity.AddClaim(new Claim(ClaimTypes.Name, usuario.Nombre));
            identity.AddClaim(new Claim("Imagen", usuario.Imagen));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );
        }
    }
}
