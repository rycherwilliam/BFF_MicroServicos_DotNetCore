using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PagamentosAPI.Domain.Models;

namespace PagamentosAPI.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentosContext _context;

        public PagamentoRepository(PagamentosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pagamento>> GetPagamentosAsync()
        {
            return await _context.Pagamentos.ToListAsync();
        }

        public async Task<Pagamento> GetPagamentoByIdAsync(int id)
        {
            return await _context.Pagamentos.FindAsync(id);
        }

        public async Task AddPagamentoAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePagamentoAsync(int id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento != null)
            {
                _context.Pagamentos.Remove(pagamento);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePagamentoAsync(Pagamento pagamento)
        {            
            _context.Entry(pagamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool Any(Func<Pagamento, bool> predicate)
        {
            return _context.Pagamentos.Any(predicate);
        }

        public async Task<IEnumerable<Pagamento>> GetPagamentosDoCliente(string cpfCnpjCliente)
        {
            return await _context.Pagamentos.Where(p => p.CpfCnpjCliente == cpfCnpjCliente).ToListAsync();
        }
    }
}
