using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreriaVirtual.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Pass")]
        public byte[] Pass { get; set; }
    }
}
