using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibreriaVirtual.Models
{
    [Keyless]
    [Table("V_ESTADISTICAS_USUARIO")]
    public class EstadisticasUsuario
    {
        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("Total")]
        public int Total { get; set; }

        [Column("Series")]
        public int Series { get; set; }

        [Column("Peliculas")]
        public int Peliculas { get; set; }

        [Column("Pendientes")]
        public int Pendientes { get; set; }
    }
}
