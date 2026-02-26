using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class AccountController : Controller
    {
        private IRepositoryLibreria repo;
        private HelperPathProvider helper;

        public AccountController(IRepositoryLibreria repo, HelperPathProvider helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(string nombre, IFormFile fichero, string email, string pass)
        {
            //diferenciar las fotos por el id en el proc de la bbdd, quitar el autoincremental
            string imagen = await SubirFileAsync(fichero);

            string resultado;
            bool correcto = await repo.RegistrarseAsync(nombre, imagen, email, pass);
            if (correcto)
            {
                resultado = "Usuario registrado correctamente";
            }
            else
            {
                resultado = "Usuario ya existente";
            }

            TempData["resultado"] = resultado;
            return RedirectToAction("Login");
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

            return ruta;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {
            Usuario usuario = await repo.LoginAsync(email, pass);

            if (usuario != null)
            {
                HttpContext.Session.SetInt32("idUsuario", usuario.IdUsuario);
                HttpContext.Session.SetString("imgUsuario", usuario.Imagen);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["error"] = "Email o contraseña incorrectos, vuelva a intentarlo";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Logout(int idusuario)
        {
            HttpContext.Session.Remove("idUsuario");
            HttpContext.Session.Remove("imgUsuario");
            return RedirectToAction("Login");
        }
    }
}
