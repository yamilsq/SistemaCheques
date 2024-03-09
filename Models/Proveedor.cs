namespace SistemaChequesNuevo.Models
{
    public class Proveedor
    {
        public Proveedor()
        {
            SolicitudCheques = new HashSet<SolicitudCheque>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int TipoPersona { get; set; }
        public string? DocumentoIdentificador { get; set; }
        public decimal? Balance { get; set; }
        public string? CuentaContable { get; set; }
        public string? Estado { get; set; }
        public virtual ICollection<SolicitudCheque> SolicitudCheques { get; set; }
    }
}
