using BFFAPI.Domain.Models;
using BFFAPI.Utils;
using System.Threading.Tasks;

namespace BFFAPI.Application.Services
{
    public interface IClientePagamentoService
    {
        Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesAsync();
        Task<ServiceResponse<Cliente>> GetClienteByCpfOuCnpjAsync(string cpfOuCnpj);
        Task<ServiceResponse<IEnumerable<Pagamento>>> GetPagamentosAsync();
        Task<ServiceResponse<Pagamento>> GetPagamentoByIdPagamentoAsync(int id);
        Task<ServiceResponse<int>> GetTotalPagamentosAsync();
        Task<ServiceResponse<int>> GetTotalPagamentosAtrasadosEAVencerAsync();
        Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesPorEstadoEStatusPagamentoAsync(string estado, EstadoPagamento statusPagamento);
        Task<ServiceResponse<IEnumerable<Pagamento>>> GetPagamentosPorEstadoAsync(string estado);
        Task<ServiceResponse<IEnumerable<MediaRendaPorEstado>>> GetMediaRendaBrutaPorEstadoAsync();
        Task<ServiceResponse<bool>> VerificarSomaPagamentosExcedeRenda(string cpfOuCnpj, decimal valorPagamento);
        Task<ServiceResponse<IEnumerable<Pagamento>>> GetPagamentosDoCliente(string cpfOuCnpj);
    }
}
