using LibreriaVirtual.Models;

namespace LibreriaVirtual.Repositories
{
    public interface IRepositoryLibreria
    {
        // USUARIO
        Task<bool> Registrarse(string nombre, string imagen, string email, string password);
        Task<Usuario> Login(string email, string password);
        Task UpdateUsuario(string nombre, string imagen, string email, string password);
        Task<Usuario> FindUsuario(int idUsuario);

        // CONTENIDO PERSONAL
        Task<List<Contenido>> GetContenidosUsuario(int idUsuario);
        Task<List<Contenido>> GetCatalogoPersonal(int idUsuario);
        Task<Contenido> InsertContenido(int idUsuario, string titulo, string tipo, string genero, string imagen);
        Task VerContenido(int idContenido, int puntuacion, string opinion);
        Task ApropiarContenido(int idContenido, int idUsuario, string titulo, string tipo, string genero, string imagen);
        Task UpdateOpinion(int idContenido, int puntuacion, string opinion);
        Task DeleteContenido(int idContenido);

        // CONTENIDO PUBLICO / BUSQUEDAS
        Task<List<Contenido>> GetCatalogoPublico();
        Task<List<Contenido>> FindContenidoTipoYGenero(string tipo, string genero);

        // RECOMENDACIONES
        Task<List<Contenido>> GetRecomendacionesGenerosMasValorados(int idUsuario);
        Task<List<Contenido>> GetRecomendacionesMejorValorados(int idUsuario);
    }
}
