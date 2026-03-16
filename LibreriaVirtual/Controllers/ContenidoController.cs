using LibreriaVirtual.Filter;
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

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Index(bool personal, string accion, bool? favs, string id)
        {
            ViewData["personal"] = personal;
            ViewData["favs"] = favs;

            List<Contenido> contenidos = new List<Contenido>();
            if (accion != null)
            {
                if (accion == "genero")
                {
                    contenidos = await repo.GetRecomendacionesGenerosMasValoradosAsync(int.Parse(id));
                }
                else if (accion == "puntuacion")
                {
                    contenidos = await repo.GetRecomendacionesMejorValoradosAsync(int.Parse(id));
                }
                else if (accion == "aleatorio")
                {
                    List<Contenido> aux = await repo.GetCatalogoPublicoAsync(int.Parse(id));

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
                    contenidos = await repo.GetCatalogoPersonalAsync(int.Parse(id));
                }
                else
                {
                    contenidos = await repo.GetCatalogoPublicoAsync(int.Parse(id));
                }
            }

            return View(contenidos);
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Details(int idcontenido)
        {
            Contenido contenido = await repo.FindContenidoAsync(idcontenido);
            return View(contenido);
        }

        [AuthorizeUsuariosAttribute]
        public IActionResult Recomendaciones()
        {
            return View();
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Ver(int idcontenido, int puntuacion, string opinion, string id)
        {
            await repo.VerContenidoAsync(idcontenido, puntuacion, opinion);
            return RedirectToAction("Index", new { personal = true, id = id });
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Delete(int idcontenido, string id)
        {
            await repo.DeleteContenidoAsync(idcontenido, int.Parse(id));
            return RedirectToAction("Index", new { personal = true, id = id });
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> UpdateOpinion(int idcontenido, int puntuacion, string opinion, string id)
        {
            await repo.UpdateOpinionAsync(idcontenido, int.Parse(id), puntuacion, opinion);
            return RedirectToAction("Index", new { personal = true, id = id });
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> Apropiar(string titulo, string tipo, string genero, string imagen, string id)
        {
            await repo.ApropiarContenidoAsync(int.Parse(id), titulo, tipo, genero, imagen);
            return RedirectToAction("Index", new { personal = false, id = id });
        }

        [AuthorizeUsuariosAttribute]
        public async Task<IActionResult> MarcarFav(int idcontenido, bool? favs, string id)
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

            return RedirectToAction("Index", new { personal = true, favs = favs, id = id });
        }

        [AuthorizeUsuariosAttribute]
        [HttpPost]
        public async Task<IActionResult> Index(string tipo, string genero, bool personal, string id)
        {
            ViewData["personal"] = personal;
            List<Contenido> contenidos = new List<Contenido>();
            contenidos = await repo.FindContenidoTipoYGeneroAsync(tipo, genero, int.Parse(id));
            return View(contenidos);
        }

        [AuthorizeUsuariosAttribute]
        [HttpPost]
        public async Task<IActionResult> Create(string titulo, string tipo, string genero, IFormFile fichero, string id)
        {
            string imagen = await SubirFileAsync(fichero, id);

            await repo.InsertContenidoAsync(int.Parse(id), titulo, tipo, genero, imagen);
            return RedirectToAction("Index", new { personal = true, id = id });
        }

        private async Task<string> SubirFileAsync(IFormFile fichero, string id)
        {
            string file = int.Parse(id) + "_" + fichero.FileName;
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
