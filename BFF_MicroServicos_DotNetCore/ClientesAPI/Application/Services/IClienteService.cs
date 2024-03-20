using System.Collections.Generic;
using System.Threading.Tasks;
using ClientesAPI.Domain.Models;

namespace ClientesAPI.Application.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetClientesAsync();
        Task<Cliente> GetClienteByCpfOuCnpjAsync(string cpfOuCnpj);
        Task AddClienteAsync(Cliente cliente);
        Task UpdateClienteAsync(string cpfOuCnpj, Cliente cliente);
        Task DeleteClienteAsync(string cpfOuCnpj);
    }
}
