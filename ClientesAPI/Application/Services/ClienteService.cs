using ClientesAPI.Domain.Models;
using ClientesAPI.Infrastructure.Repositories;
using ClientesAPI.Utils;

namespace ClientesAPI.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            var clientes = await _clienteRepository.GetClientesAsync();
            if (clientes == null)
            {
                throw new HttpResponseException("Não existem clientes cadastrados.", 404);
            }
            return clientes;            
        }

        public async Task<Cliente> GetClienteByCpfOuCnpjAsync(string cpfOuCnpj)
        {
            var cliente = await _clienteRepository.GetClienteByCpfOuCnpjAsync(cpfOuCnpj);
            if (cliente == null)
            {
                throw new HttpResponseException("Cliente não encontrado.", 404);
            }
            return cliente;
        }

        public async Task AddClienteAsync(Cliente cliente)
        {
            try { 
                await _clienteRepository.AddClienteAsync(cliente);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException("Ocorreu um erro ao tentar cadastrar o cliente. Detalhes do erro: " + ex.Message , 404);
            }
        }

        public async Task UpdateClienteAsync(string cpfOuCnpj, Cliente cliente)
        {
            var existingCliente = await _clienteRepository.GetClienteByCpfOuCnpjAsync(cpfOuCnpj);
            if (existingCliente == null)
            {
                throw new HttpResponseException("Cliente não encontrado.", 404);
            }

            existingCliente.CpfOuCnpj = cliente.CpfOuCnpj;
            existingCliente.Nome = cliente.Nome;
            existingCliente.NumeroDoContrato = cliente.NumeroDoContrato;
            existingCliente.Cidade = cliente.Cidade;
            existingCliente.Estado = cliente.Estado;
            existingCliente.RendaBruta = cliente.RendaBruta;

            try
            {
                await _clienteRepository.UpdateClienteAsync(existingCliente);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException("Ocorreu um erro ao atualizar o cliente. Detalhes do erro: " + ex.Message, 500);
            }
        }

        public async Task DeleteClienteAsync(string cpfOuCnpj)
        {
            var cliente = await _clienteRepository.GetClienteByCpfOuCnpjAsync(cpfOuCnpj);
            if (cliente == null)
            {
                throw new HttpResponseException("Cliente não encontrado.", 404);
            }

            await _clienteRepository.DeleteClienteAsync(cpfOuCnpj);
        }
    }
}
