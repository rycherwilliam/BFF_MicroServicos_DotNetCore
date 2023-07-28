namespace BFFAPI.Application.Services.Mensageria
{
    public interface IKafkaProducerService
    {
        Task ProduceEventoCadastroCliente(EventoCadastroCliente eventoCadastroCliente);
        Task ProduceEventoCadastroPagamento(EventoCadastroPagamento eventoCadastroPagamento);
    }
}
