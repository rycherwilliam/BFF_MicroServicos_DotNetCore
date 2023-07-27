using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientesAPI.Domain.Models;

namespace ClientesAPI.Infrastructure.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetClientesAsync();
        Task<Cliente> GetClienteByCpfOuCnpjAsync(string cpfOuCnpj);
        Task AddClienteAsync(Cliente cliente);
        Task UpdateClienteAsync(Cliente cliente);
        Task DeleteClienteAsync(string cpfOuCnpj);
        bool Any(Func<Cliente, bool> predicate);
    }
}
