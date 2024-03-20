using BFFAPI.Domain.Models;
using BFFAPI.Utils;

namespace BFFAPI.Application.Services.ClienteWEB
{
    public interface IClienteBFFService
    {        
        Task<ServiceResponse> AddClienteAsync(Cliente cliente);
        Task<ServiceResponse> UpdateClienteAsync(string cpfOuCnpj, Cliente cliente);
        Task<ServiceResponse> DeleteClienteAsync(string cpfOuCnpj);        
    }
}
