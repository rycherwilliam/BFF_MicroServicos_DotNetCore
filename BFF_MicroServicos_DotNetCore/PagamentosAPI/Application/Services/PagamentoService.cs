using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using PagamentosAPI.Domain.Models;
using PagamentosAPI.Infrastructure.Repositories;
using PagamentosAPI.Utils;

namespace Pagamentos.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        

        public PagamentoService(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;            
        }

        public async Task<IEnumerable<Pagamento>> GetPagamentosAsync()
        {
            var pagamentos = await _pagamentoRepository.GetPagamentosAsync();
            if (pagamentos == null)
            {
                throw new HttpResponseException("Não existem pagamentos cadastrados.", 404);
            }
            return pagamentos;            
        }

        public async Task<Pagamento> GetPagamentoByIdAsync(int id)
        {
            var pagamento = await _pagamentoRepository.GetPagamentoByIdAsync(id);
            if (pagamento == null)
            {
                throw new HttpResponseException("Pagamento não encontrado.", 404);
            }
            return pagamento;
        }

        public async Task AddPagamentoAsync(Pagamento pagamento)
        {
            try
            {
                await _pagamentoRepository.AddPagamentoAsync(pagamento);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException("Ocorreu um erro ao tentar cadastrar o pagamento. Detalhes do erro: " + ex.Message, 404);
            }
        }

        public async Task UpdatePagamentoAsync(int id, Pagamento pagamento)
        {
            var existingPagamento = await _pagamentoRepository.GetPagamentoByIdAsync(id);
            if (existingPagamento == null)
            {
                throw new HttpResponseException("Pagamento não encontrado.", 404);
            }

            existingPagamento.NumeroDoContrato = pagamento.NumeroDoContrato;
            existingPagamento.Parcela = pagamento.Parcela;
            existingPagamento.Valor = pagamento.Valor;
            existingPagamento.EstadoPagamento = pagamento.EstadoPagamento;
            existingPagamento.DataVencimento = pagamento.DataVencimento;

            try { 
                await _pagamentoRepository.UpdatePagamentoAsync(existingPagamento);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException("Ocorreu um erro ao atualizar o pagamento. Detalhes do erro: " + ex.Message, 404);
            }
        }

        public async Task DeletePagamentoAsync(int id)
        {
            var pagamento = await _pagamentoRepository.GetPagamentoByIdAsync(id);
            if (pagamento == null)
            {
                throw new HttpResponseException("Pagamento não encontrado.", 404);
            }

            await _pagamentoRepository.DeletePagamentoAsync(id);
           
        }

        public async Task<IEnumerable<Pagamento>> GetPagamentosDoCliente(string cpfOuCnpj)
        {
            var pagamento = await _pagamentoRepository.GetPagamentosDoCliente(cpfOuCnpj);            
            if (pagamento == null)
            {
                throw new HttpResponseException("Pagamento não encontrado para esse cliente.", 404);
            }
            return pagamento;
        }
    }
}
