namespace SistemaChequesNuevo.Dtos
{
    public class OrigenCuentaUIMapper
    {
        public int cuentaContableId { get; set; }
        public string origenCuentaDescripcion { get; set; }
    }

    public class OrigenCuenta
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }

    public class OrigenCuentaReponse
    {
        public List<OrigenCuenta> cuentasContables { get; set; }
    }
}
