using BFFAPI.Application.Services.Mensageria;
using BFFAPI.Application.Services.PagamentoWEB;
using BFFAPI.Domain.Models;
using BFFAPI.Utils;
using BFFAPI.Utils.Const;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace BFFAPI.Application.Services.ClienteWEB
{
    public class ClienteBFFService : IClienteBFFService
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IKafkaProducerService _kafkaProducerService;
        public ClienteBFFService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IKafkaProducerService kafkaProducerService)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _kafkaProducerService = kafkaProducerService;    
        }

        public async Task<ServiceResponse> AddClienteAsync(Cliente cliente)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");                
                

                var response = await _httpClient.PostAsync($"{Urls.Clientes}/AddCliente", content);
                response.EnsureSuccessStatusCode();


                //cadastro de cliente no Kafka
                var eventoCadastroCliente = new EventoCadastroCliente
                {
                    CpfOuCnpj = cliente.CpfOuCnpj,
                    Nome = cliente.Nome,
                    NumeroDoContrato = cliente.NumeroDoContrato,
                    Cidade = cliente.Cidade,
                    Estado = cliente.Estado,
                    RendaBruta = cliente.RendaBruta
                };

                await _kafkaProducerService.ProduceEventoCadastroCliente(eventoCadastroCliente);

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
                    ErrorMessage = "Ocorreu um erro ao adicionar o cliente. Detalhes do erro: " + ex.Message
                };
            }
        }

        public async Task<ServiceResponse> UpdateClienteAsync(string cpfOuCnpj, Cliente cliente)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");                                

                var response = await _httpClient.PutAsync($"{Urls.Clientes}/UpdateCliente/{cpfOuCnpj}", content);
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
                    ErrorMessage = "Ocorreu um erro ao atualizar o cliente. Detalhes do erro: " + ex.Message
                };
            }
        }

        public async Task<ServiceResponse> DeleteClienteAsync(string cpfOuCnpj)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var tokenWithoutBearer = token.Replace("Bearer ", ""); // Garantir que o token possua apenas um prefixo "Bearer"
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenWithoutBearer);
                var response = await _httpClient.DeleteAsync($"{Urls.Clientes}/DeleteCliente/{cpfOuCnpj}");
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
                    ErrorMessage = "Ocorreu um erro ao deletar o cliente. Detalhes do erro: " + ex.Message
                };
            }
        }        
    }
}