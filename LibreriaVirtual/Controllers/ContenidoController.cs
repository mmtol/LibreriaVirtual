using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibreriaVirtual.Controllers
{
    public class ContenidoController : Controller
    {
        private IRepositoryLibreria repo;
        private HelperPathProvider helper;

        public ContenidoController(IRepositoryLibreria repo, HelperPathProvider helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        public async Task<IActionResult> Index(bool personal, string accion, bool? favs)
        {
            ViewData["personal"] = personal;
            ViewData["favs"] = favs;

            List<Contenido> contenidos = new List<Contenido>();
            if (accion != null)
            {
                if (accion == "genero")
                {
                    contenidos = await repo.GetRecomendacionesGenerosMasValoradosAsync((int)HttpContext.Session.GetInt32("idUsuario"));
                }
                else if (accion == "puntuacion")
                {
                    contenidos = await repo.GetRecomendacionesMejorValoradosAsync((int)HttpContext.Session.GetInt32("idUsuario"));
                }
                else if (accion == "aleatorio")
                {
                    List<Contenido> aux = await repo.GetCatalogoPublicoAsync((int)HttpContext.Session.GetInt32("idUsuario"));

                    if (aux != null && aux.Count > 0)
                    {
                        Random generador = new Random();
                        int aleatorio = generador.Next(0, aux.Count);

                        return RedirectToAction("Details", new { idcontenido = aux[aleatorio].IdContenido });
                    }
                }
            }
            else
            {
                if (personal)
                {
                    contenidos = await repo.GetCatalogoPersonalAsync((int)HttpContext.Session.GetInt32("idUsuario"));
                }
                else
                {
                    contenidos = await repo.GetCatalogoPublicoAsync((int)HttpContext.Session.GetInt32("idUsuario"));
                }
            }

            return View(contenidos);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Details(int idcontenido)
        {
            Contenido contenido = await repo.FindContenidoAsync(idcontenido);
            return View(contenido);
        }

        public IActionResult Recomendaciones(string accion)
        {
            return View();
        }
        public async Task<IActionResult> Ver(int idcontenido, int puntuacion, string opinion)
        {
            await repo.VerContenidoAsync(idcontenido, puntuacion, opinion);
            return RedirectToAction("Index", new { personal = true });
        }

        public async Task<IActionResult> Delete(int idcontenido)
        {
            await repo.DeleteContenidoAsync(idcontenido, (int)HttpContext.Session.GetInt32("idUsuario"));
            return RedirectToAction("Index", new { personal = true });
        }

        public async Task<IActionResult> UpdateOpinion(int idcontenido, int puntuacion, string opinion)
        {
            await repo.UpdateOpinionAsync(idcontenido, (int)HttpContext.Session.GetInt32("idUsuario"), puntuacion, opinion);
            return RedirectToAction("Index", new { personal = true });
        }

        public async Task<IActionResult> Apropiar(int idcontenido, string titulo, string tipo, string genero, string imagen)
        {
            await repo.ApropiarContenidoAsync(idcontenido, (int)HttpContext.Session.GetInt32("idUsuario"), titulo, tipo, genero, imagen);
            return RedirectToAction("Index", new { personal = false });
        }

        public async Task<IActionResult> MarcarFav(int idcontenido)
        {
            Contenido contenido = await repo.FindContenidoAsync(idcontenido);
            if (contenido.Favorito)
            {
                await repo.UpdateFavoritoContenido(idcontenido, false);
            }
            else
            {
                await repo.UpdateFavoritoContenido(idcontenido, true);
            }

            return RedirectToAction("Index", new { personal = true, favs = true });
        }

        [HttpPost]
        public async Task<IActionResult> Index(string tipo, string genero, bool personal)
        {
            ViewData["personal"] = personal;
            List<Contenido> contenidos = new List<Contenido>();
            contenidos = await repo.FindContenidoTipoYGeneroAsync(tipo, genero, (int)HttpContext.Session.GetInt32("idUsuario"));
            return View(contenidos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string titulo, string tipo, string genero, IFormFile fichero)
        {
            string imagen = await SubirFileAsync(fichero);

            await repo.InsertContenidoAsync((int)HttpContext.Session.GetInt32("idUsuario"), titulo, tipo, genero, imagen);
            return RedirectToAction("Index", new { personal = true });
        }

        private async Task<string> SubirFileAsync(IFormFile fichero)
        {
            string file = (int)HttpContext.Session.GetInt32("idUsuario") + "_" + fichero.FileName;
            string ruta = helper.MapPath(file, Carpetas.Contenidos);
            string url = helper.MapUrlPath(file, Carpetas.Contenidos);

            using (Stream stream = new FileStream(ruta, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }

            return url;
        }
    }
}
