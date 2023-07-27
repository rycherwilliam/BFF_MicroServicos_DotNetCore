using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BFFAPI.Application.Services.ClienteWEB;
using BFFAPI.Application.Services.PagamentoWEB;
using BFFAPI.Domain.Models;
using BFFAPI.Utils;
using BFFAPI.Utils.Const;
using Newtonsoft.Json;

namespace BFFAPI.Application.Services
{
    public class ClientePagamentoService : IClientePagamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClientePagamentoService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            }

        public async Task<ServiceResponse<bool>> VerificarSomaPagamentosExcedeRenda(string cpfOuCnpj, decimal valorPagamento)
        {
            var serviceResponse = new ServiceResponse<bool>();
            var clienteResponse = await GetClienteByCpfOuCnpjAsync(cpfOuCnpj);

            if (!clienteResponse.Success || clienteResponse.Data == null)
            {
                serviceResponse.ErrorMessage = "Cliente não encontrado.";
                return serviceResponse;
            }

            var pagamentosDoClienteResponse = await GetPagamentosDoCliente(cpfOuCnpj);
            if (!pagamentosDoClienteResponse.Success)
            {
                serviceResponse.ErrorMessage = "Erro ao obter os pagamentos do cliente.";
                return serviceResponse;
            }

            var pagamentosDoCliente = pagamentosDoClienteResponse.Data;
            decimal somaPagamentos = pagamentosDoCliente
                .Where(p => p.EstadoPagamento != EstadoPagamento.Pago && p.DataVencimento >= DateTime.Today) // Somente pagamentos não pagos e com data de vencimento igual ou posterior à data atual
                .Sum(p => p.Valor) + valorPagamento;

            bool excedeRenda = somaPagamentos <= clienteResponse.Data.RendaBruta;

            serviceResponse.Success = true;
            serviceResponse.Data = excedeRenda;
            return serviceResponse;
        }
        public async Task<ServiceResponse<int>> GetTotalPagamentosAsync()
        {
            var pagamentosResponse = await GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    ErrorMessage = "Erro ao obter os pagamentos."
                };
            }

            var totalPagamentos = pagamentosResponse.Data.Count();
            return new ServiceResponse<int>
            {
                Success = true,
                Data = totalPagamentos
            };
        }
        public async Task<ServiceResponse<int>> GetTotalPagamentosAtrasadosEAVencerAsync()
        {
            var pagamentosResponse = await GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    ErrorMessage = "Erro ao obter os pagamentos."
                };
            }

            var hoje = DateTime.Today;
            var totalPagamentosAtrasadosEAVencer = pagamentosResponse.Data.Count(p => p.DataVencimento < hoje && p.EstadoPagamento != EstadoPagamento.Pago);
            return new ServiceResponse<int>
            {
                Success = true,
                Data = totalPagamentosAtrasadosEAVencer
            };
        }

        public async Task<ServiceResponse<IEnumerable<MediaRendaPorEstado>>> GetMediaRendaBrutaPorEstadoAsync()
        {
            var clientesResponse = await GetClientesAsync();
            if (!clientesResponse.Success)
            {
                return new ServiceResponse<IEnumerable<MediaRendaPorEstado>>
                {
                    Success = false,
                    ErrorMessage = "Erro ao obter os clientes."
                };
            }

            var clientes = clientesResponse.Data;
            var pagamentosResponse = await GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                return new ServiceResponse<IEnumerable<MediaRendaPorEstado>>
                {
                    Success = false,
                    ErrorMessage = "Erro ao obter os pagamentos."
                };
            }

            var pagamentos = pagamentosResponse.Data.Where(p => p.EstadoPagamento != EstadoPagamento.Pago);
            var mediaRendaPorEstado = clientes.GroupBy(c => c.Estado)
                .Select(g => new MediaRendaPorEstado
                {
                    Estado = g.Key,
                    MediaRenda = g.Average(c => c.RendaBruta)
                });

            return new ServiceResponse<IEnumerable<MediaRendaPorEstado>>
            {
                Success = true,
                Data = mediaRendaPorEstado
            };
        }

        public async Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesPorEstadoEStatusPagamentoAsync(string estado, EstadoPagamento statusPagamento)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<Cliente>>();

            var clientesResponse = await GetClientesAsync();
            if (!clientesResponse.Success)
            {
                serviceResponse.ErrorMessage = "Erro ao obter os clientes.";
                return serviceResponse;
            }

            var clientes = clientesResponse.Data;

            var pagamentosResponse = await GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                serviceResponse.ErrorMessage = "Erro ao obter os pagamentos.";
                return serviceResponse;
            }

            var pagamentos = pagamentosResponse.Data;

            var clientesPorEstadoEStatusPagamento = clientes
                .Join(pagamentos,
                    cliente => cliente.CpfOuCnpj,
                    pagamento => pagamento.CpfCnpjCliente,
                    (cliente, pagamento) => new { Cliente = cliente, Pagamento = pagamento })
                .Where(c => c.Cliente.Estado == estado && c.Pagamento.EstadoPagamento == statusPagamento)
                .Select(c => c.Cliente);

            serviceResponse.Success = true;
            serviceResponse.Data = clientesPorEstadoEStatusPagamento;
            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<Pagamento>>> GetPagamentosPorEstadoAsync(string estado)
        {
            var clientesResponse = await GetClientesAsync();
            if (!clientesResponse.Success)
            {
                return new ServiceResponse<IEnumerable<Pagamento>>
                {
                    Success = false,
                    ErrorMessage = "Erro ao obter os clientes."
                };
            }

            var clientes = clientesResponse.Data;
            var pagamentosResponse = await GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                return new ServiceResponse<IEnumerable<Pagamento>>
                {
                    Success = false,
                    ErrorMessage = "Erro ao obter os pagamentos."
                };
            }

            var pagamentos = pagamentosResponse.Data;
            var clientesPorEstado = clientes.Where(c => c.Estado == estado).Select(c => c.CpfOuCnpj);
            var pagamentosPorEstado = pagamentos.Where(p => clientesPorEstado.Contains(p.CpfCnpjCliente) && p.EstadoPagamento != EstadoPagamento.Pago);

            return new ServiceResponse<IEnumerable<Pagamento>>
            {
                Success = true,
                Data = pagamentosPorEstado
            };
        }

        public async Task<ServiceResponse<IEnumerable<Pagamento>>> GetPagamentosAsync()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var response = await _httpClient.GetAsync($"{Urls.Pagamentos}/GetPagamentos");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var pagamentos = JsonConvert.DeserializeObject<IEnumerable<Pagamento>>(content);

                return new ServiceResponse<IEnumerable<Pagamento>>
                {
                    Success = true,
                    Data = pagamentos
                };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse<IEnumerable<Pagamento>>
                {
                    Success = false,
                    ErrorMessage = "Ocorreu um erro ao buscar os pagamentos. Detalhes do erro: " + ex.Message
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesAsync()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var response = await _httpClient.GetAsync($"{Urls.Clientes}/GetClientes");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<IEnumerable<Cliente>>(content);

                return new ServiceResponse<IEnumerable<Cliente>>
                {
                    Success = true,
                    Data = clientes
                };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse<IEnumerable<Cliente>>
                {
                    Success = false,
                    ErrorMessage = "Ocorreu um erro ao buscar os clientes. Detalhes do erro: " + ex.Message
                };
            }
        }
               
        public async Task<ServiceResponse<Pagamento>> GetPagamentoByIdPagamentoAsync(int idPagamento)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var response = await _httpClient.GetAsync($"{Urls.Pagamentos}/GetPagamentoBycpfOuCnpj/{idPagamento}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var pagamento = JsonConvert.DeserializeObject<Pagamento>(content);

                return new ServiceResponse<Pagamento>
                {
                    Success = true,
                    Data = pagamento
                };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse<Pagamento>
                {
                    Success = false,
                    ErrorMessage = "Ocorreu um erro ao buscar o pagamento. Detalhes do erro: " + ex.Message
                };
            }
        }
        public async Task<ServiceResponse<Cliente>> GetClienteByCpfOuCnpjAsync(string cpfOuCnpj)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var response = await _httpClient.GetAsync($"{Urls.Clientes}/GetClienteByCpfOuCnpj/{cpfOuCnpj}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var cliente = JsonConvert.DeserializeObject<Cliente>(content);

                return new ServiceResponse<Cliente>
                {
                    Success = true,
                    Data = cliente
                };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse<Cliente>
                {
                    Success = false,
                    ErrorMessage = "Ocorreu um erro ao buscar o cliente. Detalhes do erro: " + ex.Message
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<Pagamento>>> GetPagamentosDoCliente(string cpfOuCnpj)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<Pagamento>>();

            var pagamentosResponse = await GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                serviceResponse.ErrorMessage = "Erro ao obter os pagamentos.";
                return serviceResponse;
            }

            var pagamentosDoCliente = pagamentosResponse.Data
                .Where(p => p.CpfCnpjCliente == cpfOuCnpj)
                .ToList();

            serviceResponse.Success = true;
            serviceResponse.Data = pagamentosDoCliente;
            return serviceResponse;
        }
    }
}
