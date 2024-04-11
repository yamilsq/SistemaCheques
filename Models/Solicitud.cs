namespace SistemaChequesNuevo.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int? NumeroCheque { get; set; }
    }
}
