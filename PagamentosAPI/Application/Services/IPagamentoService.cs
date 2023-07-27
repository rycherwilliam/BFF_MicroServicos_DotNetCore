using System.Collections.Generic;
using System.Threading.Tasks;
using PagamentosAPI.Domain.Models;

namespace Pagamentos.Application.Services
{
    public interface IPagamentoService
    {
        Task<IEnumerable<Pagamento>> GetPagamentosAsync();
        Task<Pagamento> GetPagamentoByIdAsync(int id);
        Task AddPagamentoAsync(Pagamento pagamento);
        Task UpdatePagamentoAsync(int id, Pagamento pagamento);
        Task DeletePagamentoAsync(int id);
        Task <IEnumerable<Pagamento>> GetPagamentosDoCliente(string cpfOuCnpj);
    }
}
