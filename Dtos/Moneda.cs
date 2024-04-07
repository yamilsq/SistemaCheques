namespace SistemaChequesNuevo.Dtos
{
    public class Moneda
    {
        public int Id { get; set; }
        public string CodigoIso { get; set; }
        public string Descripcion { get; set; }
        public double TasaCambio { get; set; }
    }

    public class MonedasResponse
    {
        public List<Moneda> Monedas { get; set; }
    }
}
