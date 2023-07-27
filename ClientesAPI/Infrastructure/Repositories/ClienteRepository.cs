using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientesAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientesAPI.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClientesContext _context;

        public ClienteRepository(ClientesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> GetClienteByCpfOuCnpjAsync(string cpfOuCnpj)
        {
            return await _context.Clientes.FindAsync(cpfOuCnpj);
        }

        public async Task AddClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClienteAsync(string cpfOuCnpj)
        {
            var cliente = await _context.Clientes.FindAsync(cpfOuCnpj);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateClienteAsync(Cliente cliente)
        {            
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool Any(Func<Cliente, bool> predicate)
        {
            return _context.Clientes.Any(predicate);
        }
    }
}
