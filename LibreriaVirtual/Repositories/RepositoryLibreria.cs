using LibreriaVirtual.Data;
using LibreriaVirtual.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.Metrics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaVirtual.Repositories
{
    #region STORED PROCEDURES
    //create or alter procedure SP_CATALOGO_PUBLICO
    //as
    //begin
    //    select Titulo, Tipo, Genero, Imagen
    //    from Contenido
    //    group by Titulo, Tipo, Genero, Imagen
    //    order by Titulo;
    //end
    //go

    //    create or alter procedure SP_REGISTRARSE
    //(@nombre nvarchar(100), @imagen nvarchar(100), @email nvarchar(150), @pass nvarchar(255), @registroExitoso bit output)
    //as
    //begin
    //    set @registroExitoso = 0;

    //    if not exists(select 1 from Usuarios where Email = @email)
    //    begin
    //        insert into Usuarios(Nombre, Imagen, Email, Pass) values(@nombre, @imagen, @email, @pass);
    //    set @registroExitoso = 1;
    //    end
    //end
    //go

    //create or alter procedure SP_LOGIN_USUARIO
    //(@email nvarchar(150),@pass nvarchar(255))
    //as
    //begin
    //    select IdUsuario, Nombre, Imagen, Email, Pass
    //    from Usuarios
    //    where Email = @email and Pass = @pass;
    //end
    //go

    //create or alter procedure SP_RECOMENDACION_GENERO_FAVORITO
    //(@idUsuario int)
    //as
    //begin
    //    declare @fav nvarchar(50);

    //    select top 1 @fav = Genero
    //    from Contenido
    //    where IdUsuario = @idUsuario and Estado = 'Vista'
    //    group by Genero
    //    order by AVG(Puntuacion) desc;

    //    select Titulo, Tipo, Genero, Imagen
    //    from Contenido
    //    where Genero = @fav and Titulo not in (select Titulo from Contenido where IdUsuario = @idUsuario)
    //    group by Titulo, Tipo, Genero, Imagen
    //    order by Titulo;
    //    end
    //    go

    //create or alter procedure SP_RECOMENDACIONES_MEJOR_VALORADOS
    //(@idUsuario int)
    //as
    //begin
    //    select Titulo, Tipo, Genero, Imagen
    //    from Contenido
    //    where Estado = 'Vista' and Puntuacion >= 8 and IdUsuario<> @idUsuario
    //    group by Titulo, Tipo, Genero, Imagen
    //    order by Titulo;
    //    end
    //    go

    //create or alter procedure SP_INSERT_CONTENIDO
    //(@idUsuario int, @titulo nvarchar(150), @tipo nvarchar(20), @genero nvarchar(50), @imagen nvarchar(200))
    //as
    //begin
    //    if not exists(select 1 from Contenido where IdUsuario = @idUsuario and Titulo = @titulo)
    //    begin
    //        insert into Contenido(IdUsuario, Titulo, Tipo, Genero, Imagen, Estado)
    //        values(@idUsuario, @titulo, @tipo, @genero, @imagen, 'Pendiente');
    //    end

    //    select IdContenido, IdUsuario, Titulo, Tipo, Genero, Imagen, Estado, Puntuacion, Opinion
    //    from Contenido
    //    where IdUsuario = @idUsuario and Titulo = @titulo;
    //end
    //go

    //create or alter procedure SP_VER_CONTENIDO
    //(@idContenido int, @puntuacion int, @opinion nvarchar(500))
    //as
    //begin
    //    update Contenido
    //    set Estado = 'Vista', Puntuacion = @puntuacion, Opinion = @opinion
    //    where IdContenido = @idContenido;
    //end
    //go

    //create or alter procedure SP_APROPIAR_CONTENIDO
    //(@idContenido int, @idUsuario int, @titulo nvarchar(150), @tipo nvarchar(20), @genero nvarchar(50), @imagen nvarchar(200))
    //as
    //begin
    //    if not exists(select 1 from Contenido where IdUsuario = @idUsuario and Titulo = @titulo)
    //    begin
    //        insert into Contenido(IdUsuario, Titulo, Tipo, Genero, Imagen, Estado)
    //        values(@idUsuario, @titulo, @tipo, @genero, @imagen, 'Pendiente');
    //    end
    //end
    //go
    #endregion

    public class RepositoryLibreria : IRepositoryLibreria
    {
        private LibreriaVirtualContext context;

        public RepositoryLibreria(LibreriaVirtualContext context)
        {
            this.context = context;
        }

        public async Task<bool> Registrarse(string nombre, string imagen, string email, string pass)
        {
            //con el procedimiento verifica si ya existe el usuario y si no, se inserta
            //devuelve true si se ha creado correctamente y false si ya existe y no se ha creado

            string sql = "SP_REGISTRARSE @nombre, @imagen, @email, @pass, @registroExitoso output";
            SqlParameter pamNombre = new SqlParameter("@nombre", nombre);
            SqlParameter pamImagen = new SqlParameter("@imagen", imagen);
            SqlParameter pamEmail = new SqlParameter("@email", email);
            SqlParameter pamPass = new SqlParameter("@pass", pass);

            SqlParameter pamExito = new SqlParameter("@registroExitoso", System.Data.SqlDbType.Bit);
            pamExito.Direction = System.Data.ParameterDirection.Output;

            await context.Database.ExecuteSqlRawAsync(sql, pamNombre, pamImagen, pamEmail, pamPass, pamExito);

            return (bool)pamExito.Value;
        }

        public async Task<Usuario> Login(string email, string pass)
        {
            //con el procedimiento almacenado verifica si el usuario existe y si la contraseña es correcta
            //si el usuario existe y la contraseña es correcta, se devuelve el usuario, sino se devuelve null

            string sql = "SP_LOGIN_USUARIO @email, @pass";
            SqlParameter pamEmail = new SqlParameter("@email", email);
            SqlParameter pamPass = new SqlParameter("@pass", pass);

            Usuario usuario = await context.Usuarios.FromSqlRaw(sql, pamEmail, pamPass).FirstOrDefaultAsync();

            return usuario; //si es null es que no existe !!!!
        }

        public async Task UpdateUsuario(string nombre, string imagen, string email, string pass)
        {
            //hay que actualizar el usuario
        }

        public async Task<Usuario> FindUsuario(int idUsuario) //??????????????????????
        {
            //devuelve el usuario por su idUsuario
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

            string sql = "SP_CATALOGO_PUBLICO";
            var consulta = context.Contenidos.FromSqlRaw(sql);
            List<Contenido> catalogoPublico = await consulta.ToListAsync();

            return catalogoPublico;
        }

        public async Task<List<Contenido>> GetCatalogoPersonal(int idUsuario)
        {
            //devuelve el catalogo personal del usuario
            return new List<Contenido>();
        }

        public async Task<Contenido> InsertContenido(int idUsuario, string titulo, string tipo, string genero, string imagen)
        {
            //con el procedimiento inserta un contenido pendiente en el catalogo personal del usuario
            //al crearlo, lo devuelve para seguir editandolo

            string sql = "SP_INSERT_CONTENIDO @idUsuario, @titulo, @tipo, @genero, @imagen";
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);
            SqlParameter pamTitulo = new SqlParameter("@titulo", titulo);
            SqlParameter pamTipo = new SqlParameter("@tipo", tipo);
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            SqlParameter pamImagen = new SqlParameter("@imagen", imagen);

            var consulta = context.Contenidos.FromSqlRaw(sql, pamIdUsuario, pamTitulo, pamTipo, pamGenero, pamImagen);
            Contenido contenido = await consulta.FirstOrDefaultAsync();
    
            return contenido;
        }

        public async Task VerContenido(int idContenido, int puntuacion, string opinion)
        {
            //con el procedimiento actualiza el contenido del catalogo personal del usuario a visto, con la puntuacion y opinion
        }

        public async Task ApropiarContenido(int idContenido, int idUsuario, string titulo, string tipo, string genero, string imagen)
        {
            //con el procedimiento, al escoger un contenido del catalogo publico, se mete al catalogo personal del usuario, con el estado pendiente
        }

        public async Task UpdateOpinion(int idContenido, int puntuacion, string opinion)
        {
            //FALTA PROCEDIMIENTO ALMACENADO
            //actualiza la opinion y puntuacion de un contenido del catalogo personal del usuario
        }

        public async Task DeleteContenido(int idContenido)
        {
            //FALTA PROCEDIMIENTO ALMACENADO
            //elimina un contenido del catalogo personal del usuario
        }

        public async Task<List<Contenido>> GetRecomendacionesGenerosMasValorados(int IdUsuario)
        {
            //con el procedimiento, devuelve una lista de contenidos del catalogo publico, que sean del genero mas valorado por el usuario

            string sql = "SP_RECOMENDACION_GENERO_FAVORITO @idUsuario";
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", IdUsuario);

            var consulta = context.Contenidos.FromSqlRaw(sql, pamIdUsuario);
            List<Contenido> recomendacionesGenero = await consulta.ToListAsync();

            return recomendacionesGenero;
        }

        public async Task<List<Contenido>> GetRecomendacionesMejorValorados(int idUsuario)
        {
            //con el procedimiento devuelve una lista de contenidos del catalogo publico, que sean todo notas 8/9/10

            string sql = "SP_RECOMENDACIONES_MEJOR_VALORADOS @idUsuario";
            SqlParameter pamIdUsuario = new SqlParameter("@idUsuario", idUsuario);

            var consulta = context.Contenidos.FromSqlRaw(sql, pamIdUsuario);
            List<Contenido> recomendacionesMejorValorados = await consulta.ToListAsync();

            return recomendacionesMejorValorados;
        }
    }
}
