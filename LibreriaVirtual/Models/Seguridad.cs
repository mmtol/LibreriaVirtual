using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreriaVirtual.Models
{
    [Table("Seguridad")]
    public class Seguridad
    {
        [Key]
        [Column("IdSeguridad")]
        public int IdSeguridad { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Salt")]
        public string Salt { get; set; }
    }
}
