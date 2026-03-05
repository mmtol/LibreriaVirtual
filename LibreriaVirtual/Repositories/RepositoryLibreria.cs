using LibreriaVirtual.Data;
using LibreriaVirtual.Helpers;
using LibreriaVirtual.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibreriaVirtual.Repositories
{
    public class RepositoryLibreria : IRepositoryLibreria
    {
        private LibreriaVirtualContext context;

        public RepositoryLibreria(LibreriaVirtualContext context)
        {
            this.context = context;
        }

        public async Task<int> GetMaxIdUsuariosAsync()
        {
            if (context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }

        public async Task<int> GetMaxIdContenidosAsync()
        {
            if (context.Contenidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await context.Contenidos.MaxAsync(z => z.IdContenido) + 1;
            }
        }

        public async Task<bool> RegistrarseAsync(string nombre, string imagen, string email, string pass)
        {
            //con el procedimiento verifica si ya existe el usuario y si no, se inserta
            //devuelve true si se ha creado correctamente y false si ya existe y no se ha creado

            int idUsuario = await GetMaxIdUsuariosAsync();

            string salt = HelperTools.GenerarSalt();
            byte[] passHash = HelperCrytography.EncriptarPass(pass, salt);

            string sql = "SP_REGISTRARSE @idUsuario, @nombre, @imagen, @email, @pass, @salt, @registroExitoso output";
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamImagen = new SqlParameter("@imagen", imagen);
            SqlParameter pamEmail = new SqlParameter("@email", email);
            SqlParameter pamPass = new SqlParameter("@pass", passHash);
            SqlParameter pamSalt = new SqlParameter("@salt", salt);

            SqlParameter pamExito = new SqlParameter("@registroExitoso", System.Data.SqlDbType.Bit);
            pamExito.Direction = System.Data.ParameterDirection.Output;

            await context.Database.ExecuteSqlRawAsync(sql, pamIdUsuario, pamNombre, pamImagen, pamEmail, pamPass, pamSalt, pamExito);

            return (bool)pamExito.Value;
        }

        public async Task<string> FindSaltAsync(int idUsuario)
        {
            //devuelve el salt de un usuario
            var consulta = from datos in context.Seguridads
                           where datos.IdUsuario == idUsuario
                           select datos;
            Seguridad seguridad = await consulta.FirstOrDefaultAsync();
            return seguridad.Salt;
        }

        public async Task<Usuario> LoginAsync(string email, string pass)
        {
            //con el procedimiento almacenado verifica si el usuario existe y si la contraseña es correcta
            //si el usuario existe y la contraseña es correcta, se devuelve el usuario, sino se devuelve null

            Usuario encontrado = await FindUsuarioEmailAsync(email);

            if (encontrado == null)
            {
                return null;
            }
            else
            {
                string salt = await FindSaltAsync(encontrado.IdUsuario);
                byte[] passHash = HelperCrytography.EncriptarPass(pass, salt);

                string sql = "SP_LOGIN_USUARIO @pass";
                SqlParameter pamPass = new SqlParameter("@pass", passHash);

                Usuario usuario = context.Usuarios.FromSqlRaw(sql, pamPass).AsEnumerable().FirstOrDefault();

                return usuario;
            }
        }

        public async Task UpdateUsuarioAsync(int idUsuario, string nombre, string imagen, string email)
        {
            //hay que actualizar el usuario
            Usuario usuario = await FindUsuarioIdAsync(idUsuario);

            usuario.Nombre = nombre;
            usuario.Imagen = imagen;
            usuario.Email = email;

            await context.SaveChangesAsync();
        }

        public async Task<Usuario> FindUsuarioEmailAsync(string email)
        {
            //devuelve el usuario por su idUsuario
            var consulta = from datos in context.Usuarios
                           where datos.Email == email
                           select datos;
            Usuario usuario = await consulta.FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<Usuario> FindUsuarioIdAsync(int idUsuario)
        {
            //devuelve el usuario por su idUsuario
            var consulta = from datos in context.Usuarios
                           where datos.IdUsuario == idUsuario
                           select datos;
            Usuario usuario = await consulta.FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<List<Contenido>> FindContenidoTipoYGeneroAsync(string tipo, string genero, int idUsuario)
        {
            var titulosUsuario = from datos in context.Contenidos
                                 where datos.IdUsuario == idUsuario
                                 select datos.Titulo;

            var consulta = from datos in context.Contenidos
                           where !titulosUsuario.Contains(datos.Titulo)
                           select datos;

            if (tipo != null && tipo != "-- Tipo --")
            {
                consulta = from datos in consulta
                           where datos.Tipo == tipo
                           select datos;
            }

            if (genero != null && genero != "-- Género --")
            {
                consulta = from datos in consulta
                           where datos.Genero == genero
                           select datos;
            }

            List<Contenido> resultados = await consulta.ToListAsync();
            return resultados;
        }

        public async Task<List<Contenido>> GetCatalogoPublicoAsync(int idUsuario)
        {
            //primero saco los titulos que ya tiene el usuario
            var titulosUsuario = from datos in context.Contenidos
                                 where datos.IdUsuario == idUsuario
                                 select datos.Titulo;

            //y los saco de todos los contenidos para que me muestre los del resto
            var consulta = from datos in context.Contenidos
                           where !titulosUsuario.Contains(datos.Titulo)
                           group datos by datos.Titulo into grupo
                           select grupo.First();

            List<Contenido> catalogoPublico = await consulta.ToListAsync();

            return catalogoPublico;
        }

        public async Task<List<Contenido>> GetCatalogoPersonalAsync(int idUsuario)
        {
            //devuelve el catalogo personal del usuario
            var consulta = from datos in context.Contenidos
                           where datos.IdUsuario == idUsuario
                           select datos;
            List<Contenido> catalogoPersonal = await consulta.ToListAsync();
            return catalogoPersonal;
        }

        public async Task<Contenido> InsertContenidoAsync(int idUsuario, string titulo, string tipo, string genero, string imagen)
        {
            //con el procedimiento inserta un contenido pendiente en el catalogo personal del usuario
            //al crearlo, lo devuelve para seguir editandolo

            int idContenido = await GetMaxIdContenidosAsync();

            string sql = "SP_INSERT_CONTENIDO @idContenido, @idUsuario, @titulo, @tipo, @genero, @imagen";
            SqlParameter pamIdContenido = new SqlParameter("@idContenido", idContenido);
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter pamTitulo = new SqlParameter("@titulo", titulo);
            SqlParameter pamTipo = new SqlParameter("@tipo", tipo);
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            SqlParameter pamImagen = new SqlParameter("@imagen", imagen);

            Contenido contenido = context.Contenidos
                                .FromSqlRaw(sql, pamIdContenido, pamIdUsuario, pamTitulo, pamTipo, pamGenero, pamImagen)
                                .AsEnumerable()
                                .FirstOrDefault();
            return contenido;
        }

        public async Task VerContenidoAsync(int idContenido, int puntuacion, string opinion)
        {
            //con el procedimiento actualiza el contenido del catalogo personal del usuario a visto, con la puntuacion y opinion
            string sql = "SP_VER_CONTENIDO @idContenido, @puntuacion, @opinion";
            SqlParameter pamIdContenido = new SqlParameter("@idContenido", idContenido);
            SqlParameter pamPuntuacion = new SqlParameter("@puntuacion", puntuacion);
            SqlParameter pamOpinion = new SqlParameter("@opinion", opinion);

            await context.Database.ExecuteSqlRawAsync(sql, pamIdContenido, pamPuntuacion, pamOpinion);
        }

        public async Task ApropiarContenidoAsync(int idContenido, int idUsuario, string titulo, string tipo, string genero, string imagen)
        {
            //con el procedimiento, al escoger un contenido del catalogo publico, se mete al catalogo personal del usuario, con el estado pendiente
            string sql = "SP_APROPIAR_CONTENIDO @idContenido, @idUsuario, @titulo, @tipo, @genero, @imagen";
            SqlParameter pamIdContenido = new SqlParameter("@idContenido", idContenido);
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter pamTitulo = new SqlParameter("@titulo", titulo);
            SqlParameter pamTipo = new SqlParameter("@tipo", tipo);
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            SqlParameter pamImagen = new SqlParameter("@imagen", imagen);

            await context.Database.ExecuteSqlRawAsync(sql, pamIdContenido, pamIdUsuario, pamTitulo, pamTipo, pamGenero, pamImagen);
        }

        public async Task<Contenido> FindContenidoAsync(int idContenido)
        {
            //saca un Contenido segun su id
            var consulta = from datos in context.Contenidos
                           where datos.IdContenido == idContenido
                           select datos;
            Contenido contenido = await consulta.FirstOrDefaultAsync();
            return contenido;
        }

        public async Task UpdateOpinionAsync(int idContenido, int idUsuario, int puntuacion, string opinion)
        {
            //con el procedimmiento actualiza la opinion y puntuacion de un contenido del catalogo personal del usuario
            string sql = "SP_UPDATE_OPINION @idContenido, @idUsuario, @puntuacion, @opinion";
            SqlParameter pamIdContenido = new SqlParameter("@idContenido", idContenido);
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter pamPuntuacion = new SqlParameter("@puntuacion", puntuacion);
            SqlParameter pamOpinion = new SqlParameter("@opinion", opinion);

            await context.Database.ExecuteSqlRawAsync(sql, pamIdContenido, pamIdUsuario, pamPuntuacion, pamOpinion);
        }

        public async Task DeleteContenidoAsync(int idContenido, int idUsuario)
        {
            //con el procedimiento elimina un contenido del catalogo personal del usuario
            string sql = "SP_DELETE_CONTENIDO @idContenido, @idUsuario";
            SqlParameter pamIdContenido = new SqlParameter("@idContenido", idContenido);
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);

            await context.Database.ExecuteSqlRawAsync(sql, pamIdContenido, pamIdUsuario);
        }

        public async Task<List<Contenido>> GetRecomendacionesGenerosMasValoradosAsync(int IdUsuario)
        {
            //con el procedimiento, devuelve una lista de contenidos del catalogo publico, que sean del genero mas valorado por el usuario

            string sql = "SP_RECOMENDACION_GENERO_FAVORITO @idUsuario";
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", IdUsuario);

            var consulta = context.Contenidos.FromSqlRaw(sql, pamIdUsuario);
            List<Contenido> recomendacionesGenero = await consulta.ToListAsync();

            return recomendacionesGenero;
        }

        public async Task<List<Contenido>> GetRecomendacionesMejorValoradosAsync(int idUsuario)
        {
            //con el procedimiento devuelve una lista de contenidos del catalogo publico, que sean todo notas 8/9/10

            string sql = "SP_RECOMENDACIONES_MEJOR_VALORADOS @idUsuario";
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);

            var consulta = context.Contenidos.FromSqlRaw(sql, pamIdUsuario);
            List<Contenido> recomendacionesMejorValorados = await consulta.ToListAsync();

            return recomendacionesMejorValorados;
        }

        public async Task<EstadisticasUsuario> GetEstadisticasUsuarioAsync(int idUsuario)
        {
            //devuelve las estadisticas del usuario de la vista creada para el perfil
            var consulta = from datos in context.EstadisticasUsuarios
                           where datos.IdUsuario == idUsuario
                           select datos;
            EstadisticasUsuario estadisticas = await consulta.FirstOrDefaultAsync();
            return estadisticas;
        }
    }
}