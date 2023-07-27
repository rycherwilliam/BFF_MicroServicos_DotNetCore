using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagamentosAPI.Domain.Models;

namespace PagamentosAPI.Infrastructure.Repositories
{
    public interface IPagamentoRepository
    {
        Task<IEnumerable<Pagamento>> GetPagamentosAsync();
        Task<Pagamento> GetPagamentoByIdAsync(int idPagamento);
        Task AddPagamentoAsync(Pagamento pagamento);
        Task UpdatePagamentoAsync(Pagamento pagamento);
        Task DeletePagamentoAsync(int id);
        bool Any(Func<Pagamento, bool> predicate);
        Task<IEnumerable<Pagamento>> GetPagamentosDoCliente(string cpfOuCnp);
    }
}
