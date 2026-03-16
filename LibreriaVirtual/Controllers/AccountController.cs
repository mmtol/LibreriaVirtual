using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            string imagen;
            if (fichero == null)
            {
                imagen = "images/users/default.jpg";
            }
            else
            {
                imagen = await SubirFileAsync(fichero);
            }

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

            return url;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {
            Usuario usuario = await repo.LoginAsync(email, pass);

            if (usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role
                );

                string nombre = usuario.Nombre;
                string imagen = usuario.Imagen;
                int idUsuario = usuario.IdUsuario;

                identity.AddClaim(new Claim(ClaimTypes.Name, nombre));
                identity.AddClaim(new Claim("Imagen", imagen));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()));

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal
                );

                var routeValues = new RouteValueDictionary();

                if (TempData["id"] != null)
                {
                    routeValues["id"] = idUsuario.ToString();
                }

                if (TempData["personal"] != null)
                {
                    routeValues["personal"] = TempData["personal"].ToString();
                }

                if (TempData["favs"] != null)
                {
                    routeValues["favs"] = TempData["favs"].ToString();
                }

                if (routeValues.Count > 0)
                {
                    return RedirectToAction("Index", "Home", routeValues);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewData["error"] = "Email o contraseña incorrectos, vuelva a intentarlo";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
