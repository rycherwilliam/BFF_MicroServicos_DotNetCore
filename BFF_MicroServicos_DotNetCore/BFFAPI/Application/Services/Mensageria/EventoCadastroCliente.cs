namespace BFFAPI.Application.Services.Mensageria
{
    public class EventoCadastroCliente
    {
        public string CpfOuCnpj { get; set; }
        public string Nome { get; set; }
        public int NumeroDoContrato { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public decimal RendaBruta { get; set; }
    }
}
