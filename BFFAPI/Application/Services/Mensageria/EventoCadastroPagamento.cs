namespace BFFAPI.Application.Services.Mensageria
{
    public class EventoCadastroPagamento
    {
        public int Id { get; set; }
        public int NumeroDoContrato { get; set; }
        public int Parcela { get; set; }
        public decimal Valor { get; set; }
        public string EstadoPagamento { get; set; }
        public string CpfCnpjCliente { get; set; }
    }   
}
