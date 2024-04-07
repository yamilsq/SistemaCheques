namespace SistemaChequesNuevo.Dtos
{
    public class TipoCuenta
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int origen { get; set; }
    }

    public class TipoCuentaReponse
    {
        public List<TipoCuenta> cuentasContables { get; set; }
    }
}
