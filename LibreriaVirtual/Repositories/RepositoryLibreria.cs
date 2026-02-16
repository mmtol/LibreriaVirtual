using LibreriaVirtual.Data;
using LibreriaVirtual.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaVirtual.Repositories
{
    #region STORED PROCEDURES
    //    CREATE OR ALTER PROCEDURE SP_CATALOGO_PUBLICO
    //AS
    //BEGIN
    //    SELECT
    //        Titulo,
    //        Tipo,
    //        Genero,
    //        Imagen
    //    FROM Contenido
    //    GROUP BY
    //        Titulo,
    //        Tipo,
    //        Genero,
    //        Imagen
    //    ORDER BY Titulo;
    //END
    //GO
    #endregion

    public class RepositoryLibreria : IRepositoryLibreria
    {
        private LibreriaVirtualContext context;

        public RepositoryLibreria(LibreriaVirtualContext context)
        {
            this.context = context;
        }

        public async Task<bool> Registrarse(string nombre, string imagen, string email, string password)
        {
            //hay que verificar si ya existe el usuario
            return true;
        }

        public async Task<Usuario> Login(string email, string password)
        {
            //hay que verificar si el usuario existe y si la contraseña es correcta
            //si el usuario existe y la contraseña es correcta, se devuelve el usuario, sino se devuelve null
            return new Usuario();
        }

        public async Task UpdateUsuario(string nombre, string imagen, string email, string password)
        {
            //hay que actualizar el usuario
        }

        public async Task<Usuario> FindUsuario(int idUsuario) //??????????????????????
        {
            //devuleve el usuario por su idUsuario
            return new Usuario();
        }

        public async Task<List<Contenido>> GetContenidosUsuario(int idUsuario)
        {
            //devuelve la lista de contenidos del usuario para el catalogo personal
            return new List<Contenido>();
        }

        public async Task<List<Contenido>> FindContenidoTipoYGenero(string tipo, string genero)
        {
            //si el tipo y/o el genero no son nulos, devuelven los contenidos que coincidan por ello
            return new List<Contenido>();
        }

        public async Task<List<Contenido>> GetCatalogoPublico()
        {
            //con el procedimiento almacenado, devuelve el catalogo publico
            return new List<Contenido>();
        }

        public async Task<List<Contenido>> GetCatalogoPersonal(int idUsuario)
        {
            //devuelve el catalogo personal del usuario
            return new List<Contenido>();
        }

        public async Task<Contenido> InsertContenido(int idUsuario, string titulo, string tipo, string genero, string imagen)
        {
            //inserta un contenido pendiente en el catalogo personal del usuario
            //al crearlo, lo devuelve para seguir editandolo
            return new Contenido();
        }

        public async Task VerContenido(int idContenido, int puntuacion, string opinion)
        {
            //actualiza el contenido del catalogo personal del usuario a visto, con la puntuacion y opinion
        }

        public async Task ApropiarContenido(int idContenido, int idUsuario, string titulo, string tipo, string genero, string imagen)
        {
            //al escoger un contenido del catalogo publico, se mete al catalogo personal del usuario, con el estado pendiente
        }

        public async Task UpdateOpinion(int idContenido, int puntuacion, string opinion)
        {
            //actualiza la opinion y puntuacion de un contenido del catalogo personal del usuario
        }

        public async Task DeleteContenido(int idContenido)
        {
            //elimina un contenido del catalogo personal del usuario
        }

        public async Task<List<Contenido>> GetRecomendacionesGenerosMasValorados()
        {
            //FALTA PROCEDIMIENTO ALMACENADO
            //devuelve una lista de contenidos del catalogo publico, que sean de los generos mas valorados por el usuario
            return new List<Contenido>();
        }

        public async Task<List<Contenido>> GetRecomendacionesMejorValorados()
        {
            //FALTA PROCEDIMIENTO ALMACENADO
            //devuelve una lista de contenidos del catalogo publico, que sean los mejor valorados por otros usuarios
            return new List<Contenido>();
        }
    }
}
