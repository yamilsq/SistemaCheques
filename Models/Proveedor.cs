using System.ComponentModel.DataAnnotations.Schema;

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
        public string DocumentoIdentificador { get; set; }
        public decimal Balance { get; set; }
        public int CuentaContable { get; set; }
        public string Estado { get; set; }
        [NotMapped]
        public string? CuentaContableDescription { get; set; }
        public virtual ICollection<SolicitudCheque> SolicitudCheques { get; set; }
    }
}