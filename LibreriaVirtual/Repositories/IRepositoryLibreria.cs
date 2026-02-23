using LibreriaVirtual.Models;

namespace LibreriaVirtual.Repositories
{
    public interface IRepositoryLibreria
    {
        // USUARIO
        Task<bool> RegistrarseAsync(string nombre, string imagen, string email, string password);
        Task<Usuario> Login(string email, string password);
        Task UpdateUsuarioAsync(string nombre, string imagen, string email, string password);
        Task<Usuario> FindUsuarioAsync(string email);

        // CONTENIDO PERSONAL
        Task<List<Contenido>> GetContenidosUsuarioAsync(int idUsuario);
        Task<List<Contenido>> GetCatalogoPersonalAsync(int idUsuario);
        Task<Contenido> InsertContenidoAsync(int idUsuario, string titulo, string tipo, string genero, string imagen);
        Task VerContenidoAsync(int idContenido, int puntuacion, string opinion);
        Task ApropiarContenidoAsync(int idContenido, int idUsuario, string titulo, string tipo, string genero, string imagen);
        Task UpdateOpinionAsync(int idContenido, int idUsuario, int puntuacion, string opinion);
        Task DeleteContenidoAsync(int idContenido, int idUsuario);

        // CONTENIDO PUBLICO / BUSQUEDAS
        Task<List<Contenido>> GetCatalogoPublicoAsync();
        Task<List<Contenido>> FindContenidoTipoYGeneroAsync(string tipo, string genero);

        // RECOMENDACIONES
        Task<List<Contenido>> GetRecomendacionesGenerosMasValoradosAsync(int idUsuario);
        Task<List<Contenido>> GetRecomendacionesMejorValoradosAsync(int idUsuario);
    }
}
