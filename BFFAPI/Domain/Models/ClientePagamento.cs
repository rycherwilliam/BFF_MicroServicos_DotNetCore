namespace BFFAPI.Domain.Models
{
    public class ClientePagamento
    {
        public Cliente Cliente { get; set; }
        public List<Pagamento> Pagamentos { get; set; }
    }

}
