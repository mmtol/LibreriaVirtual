using LibreriaVirtual.Models;
using Microsoft.EntityFrameworkCore;

namespace LibreriaVirtual.Data
{
    public class LibreriaVirtualContext: DbContext
    {
        public LibreriaVirtualContext(DbContextOptions<LibreriaVirtualContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Contenido> Contenidos { get; set; }
    }
}
