using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreriaVirtual.Models
{
    [Table("USUARIOS")]
    public class Usuario
    {
        [Key]
        [Column("IdUsuarios")]
        public int IdUsuario { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Pass")]
        public string Pass { get; set; }
    }
}
