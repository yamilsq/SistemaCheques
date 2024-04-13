using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaChequesNuevo.Models
{
    public class SolicitudCheque
    {
        public SolicitudCheque()
        {
            Solicitudes = new HashSet<SolicitudCheque>();
        }
        public int Id { get; set; }
        public int? NumeroSolicitud { get; set; }
        public decimal? Monto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Estado { get; set; }
        public int? CuentaContable { get; set; }

        [NotMapped]
        public DateTime FechaDesde { get; set; }
        [NotMapped]
        public DateTime FechaHasta { get; set; }
        [NotMapped]
        public string? CuentaContableDescription { get; set; }
        [NotMapped]
        public ICollection<SolicitudCheque> Solicitudes { get; set; }

        public string? CuentaDestino { get; set; }
        public int? ProveedorId { get; set; }
        public virtual Proveedor? Proveedor { get; set; }
    }
}