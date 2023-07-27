using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BFFAPI.Domain.Models;
using BFFAPI.Utils;
using BFFAPI.Utils.Const;
using Newtonsoft.Json;

namespace BFFAPI.Application.Services.PagamentoWEB
{
    public class PagamentoService : IPagamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly IClientePagamentoService _clientePagamentoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PagamentoService(HttpClient httpClient, IClientePagamentoService clientePagamentoService, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;        
            _clientePagamentoService = clientePagamentoService;
            _httpContextAccessor = httpContextAccessor;
        }      

        public async Task<ServiceResponse> AddPagamentoAsync(Pagamento pagamento)
        {            
            var isPagamentoValidoResponse = await _clientePagamentoService.VerificarSomaPagamentosExcedeRenda(pagamento.CpfCnpjCliente, pagamento.Valor);
            if (!isPagamentoValidoResponse.Success)
            {
                return new ServiceResponse { Success = false, ErrorMessage = "Ocorreu um erro ao verificar a regra de negócio: " + isPagamentoValidoResponse.ErrorMessage };
            }

            bool isPagamentoValido = isPagamentoValidoResponse.Data;

            if (!isPagamentoValido)
            {
                return new ServiceResponse { Success = false, ErrorMessage = "A soma dos pagamentos não pode exceder a renda bruta do cliente." };
            }

            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);

                var json = JsonConvert.SerializeObject(pagamento);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{Urls.Pagamentos}/AddPagamento", content);
                response.EnsureSuccessStatusCode();

                return new ServiceResponse { Success = true };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse { Success = false, ErrorMessage = "Ocorreu um erro ao adicionar o pagamento. Detalhes do erro: " + ex.Message };
            }
        }

        public async Task<ServiceResponse> UpdatePagamentoAsync(int id, Pagamento pagamento)
        {            
            var isPagamentoValidoResponse = await _clientePagamentoService.VerificarSomaPagamentosExcedeRenda(pagamento.CpfCnpjCliente, pagamento.Valor);
            if (!isPagamentoValidoResponse.Success)
            {
                return new ServiceResponse { Success = false, ErrorMessage = "Ocorreu um erro ao verificar a regra de negócio: " + isPagamentoValidoResponse.ErrorMessage };
            }

            bool isPagamentoValido = isPagamentoValidoResponse.Data;

            if (!isPagamentoValido)
            {
                return new ServiceResponse { Success = false, ErrorMessage = "A soma dos pagamentos não pode exceder a renda bruta do cliente." };
            }

            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var json = JsonConvert.SerializeObject(pagamento);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{Urls.Pagamentos}/UpdatePagamento/{id}", content);
                response.EnsureSuccessStatusCode();

                return new ServiceResponse { Success = true };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse { Success = false, ErrorMessage = "Ocorreu um erro ao atualizar o pagamento. Detalhes do erro: " + ex.Message };
            }
        }

        public async Task<ServiceResponse> DeletePagamentoAsync(int id)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var response = await _httpClient.DeleteAsync($"{Urls.Pagamentos}/DeletePagamento/{id}");
                response.EnsureSuccessStatusCode();

                return new ServiceResponse
                {
                    Success = true
                };
            }
            catch (HttpRequestException ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    ErrorMessage = "Ocorreu um erro ao excluir o pagamento. Detalhes do erro: " + ex.Message
                };
            }
        }        
    }
}
