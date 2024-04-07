namespace SistemaChequesNuevo.Models
{
    public class TransaccionDto
    {
        public int cuenta { get; set; }
        public int tipoMovimiento { get; set; }
        public int monto { get; set; }
    }
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

    public class AsientoContableDto
    {
        public AsientoContableDto(AsientoContable ac)
        {
            this.descripcion = ac.Descripcion;
            this.auxiliar = 9;
            this.fecha = ac.Fecha.ToString("yyyy-MM-dd");
            this.monto = ac.Transacciones.Sum(x => x.Monto);
            this.estado = ac.Estado;
            this.moneda = ac.Moneda;
            this.transacciones = new List<TransaccionDto>();
            foreach (var trans in ac.Transacciones)
            {
                var t = new TransaccionDto();
                t.cuenta = trans.Cuenta;
                t.monto = trans.Monto;
                t.tipoMovimiento = trans.TipoMovimiento;
                this.transacciones.Add(t);
            }
        }
        public string descripcion { get; set; }
        public int auxiliar { get; set; }
        public string fecha { get; set; }
        public int monto { get; set; }
        public int estado { get; set; }
        public int moneda { get; set; }
        public List<TransaccionDto> transacciones { get; set; }
    }
}
