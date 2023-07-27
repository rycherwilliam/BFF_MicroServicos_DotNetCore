using BFFAPI.Domain.Models;

public interface IMessageSender
{
    Task SendPagamentoRealizadoMessage(Pagamento pagamento);
    Task SendPagamentoAtualizadoMessage(Pagamento pagamento);
    Task SendPagamentoExcluidoMessage(Pagamento pagamento);
}
