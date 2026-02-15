using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreriaVirtual.Models
{
    [Table("CONTENIDO")]
    public class Contenido
    {
        [Key]
        [Column("IdContenido")]
        public int IdContenido { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Titulo")]
        public string Titulo { get; set; }

        [Column("Tipo")]
        public string Tipo { get; set; }

        [Column("Genero")]
        public string Genero { get; set; }

        [Column("Imagen")]
        public string Imagen { get; set; }

        [Column("Estado")]
        public string Estado { get; set; }

        [Column("Puntuacion")]
        public int Puntuacion { get; set; }

        [Column("Opinion")]
        public string Opinion { get; set; }
    }
}
