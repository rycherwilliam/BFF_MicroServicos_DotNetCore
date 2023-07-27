namespace BFFAPI.Domain.Models
{
    public class Pagamento
    {
        public int Id { get; set; }        
        public int NumeroDoContrato { get; set; }
        public int Parcela { get; set; }
        public decimal Valor { get; set; }
        public EstadoPagamento EstadoPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public string CpfCnpjCliente { get; set; }
    }

    public enum EstadoPagamento
    {
        Pago,
        Atrasado,
        A_Vencer
    }
}






