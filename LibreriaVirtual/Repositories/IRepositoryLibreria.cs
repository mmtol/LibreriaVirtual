using LibreriaVirtual.Models;

namespace LibreriaVirtual.Repositories
{
    public interface IRepositoryLibreria
    {
        // USUARIO
        Task<bool> RegistrarseAsync(string nombre, string imagen, string email, string pass);
        Task<Usuario> LoginAsync(string email, string pass);
        Task UpdateUsuarioAsync(int idUsuario, string nombre, string imagen, string email);
        Task<Usuario> FindUsuarioEmailAsync(string email);
        Task<Usuario> FindUsuarioIdAsync(int idUsuario);
        Task<EstadisticasUsuario> GetEstadisticasUsuarioAsync(int idUsuario);

        // CONTENIDO PERSONAL
        Task<List<Contenido>> GetContenidosUsuarioAsync(int idUsuario);
        Task<List<Contenido>> GetCatalogoPersonalAsync(int idUsuario);
        Task<Contenido> InsertContenidoAsync(int idUsuario, string titulo, string tipo, string genero, string imagen);
        Task VerContenidoAsync(int idContenido, int puntuacion, string opinion);
        Task ApropiarContenidoAsync(int idContenido, int idUsuario, string titulo, string tipo, string genero, string imagen);
        Task<Contenido> FindContenidoAsync(int idContenido);
        Task UpdateOpinionAsync(int idContenido, int idUsuario, int puntuacion, string opinion);
        Task DeleteContenidoAsync(int idContenido, int idUsuario);

        // CONTENIDO PUBLICO / BUSQUEDAS
        Task<List<Contenido>> GetCatalogoPublicoAsync(int idUsuario);
        Task<List<Contenido>> FindContenidoTipoYGeneroAsync(string tipo, string genero, int idUsuario);

        // RECOMENDACIONES
        Task<List<Contenido>> GetRecomendacionesGenerosMasValoradosAsync(int idUsuario);
        Task<List<Contenido>> GetRecomendacionesMejorValoradosAsync(int idUsuario);
    }
}
