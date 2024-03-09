namespace SistemaChequesNuevo.Models
{
    public class SolicitudCheque
    {
        public int Id { get; set; }
        public int NumeroSolicitud { get; set; }
        public decimal? Monto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Estado { get; set; }
        public string? CuentaContable { get; set; }
        public string? CuentaDestino { get; set; }
        public int ProveedorId { get; set; }
        public virtual Proveedor? Proveedor { get; set; }
    }
}
