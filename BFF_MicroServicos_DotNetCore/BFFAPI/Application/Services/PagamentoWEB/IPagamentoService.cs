using BFFAPI.Domain.Models;
using BFFAPI.Utils;
namespace BFFAPI.Application.Services.PagamentoWEB
{
    public interface IPagamentoService
    {     
        Task<ServiceResponse> AddPagamentoAsync(Pagamento pagamento);
        Task<ServiceResponse> UpdatePagamentoAsync(int id, Pagamento pagamento);
        Task<ServiceResponse> DeletePagamentoAsync(int id);        
    }
}