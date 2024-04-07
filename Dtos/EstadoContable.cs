namespace SistemaChequesNuevo.Dtos
{

    public class KeyValue
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }
    public class EstadoContable
    {
        public List<KeyValue> estadosContables { get; set; }

    }
}
