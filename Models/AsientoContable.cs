namespace SistemaChequesNuevo.Models
{
    public class Transaccion
    {
        public int Cuenta { get; set; }
        public int TipoMovimiento { get; set; }
        public int Monto { get; set; }
    }

    public class AsientoContable
    {
        public string Descripcion { get; set; }
        public int Auxiliar { get; set; }
        public DateTime Fecha { get; set; }
        public int Monto { get; set; }
        public int Estado { get; set; }
        public int Moneda { get; set; }
        public List<Transaccion> Transacciones { get; set; }
    }
}
