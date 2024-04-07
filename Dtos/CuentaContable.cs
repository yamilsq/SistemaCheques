namespace SistemaChequesNuevo.Dtos
{

    public class CuentaContable
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
    }

    public class CuentaContableResponse
    {
        public List<CuentaContable> cuentasContables { get; set; }
    }
}
