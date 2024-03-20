using System;
using System.Text.Json;
using System.Threading.Tasks;
using BFFAPI.Domain.Models;
using Confluent.Kafka;

namespace BFFAPI.Infrastructure
{
    public class MessageSender : IMessageSender
    {
        private readonly IProducer<Null, string> _producer;

        public MessageSender(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendPagamentoRealizadoMessage(Pagamento pagamento)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(new
                {
                    Event = "PagamentoRealizado",
                    Data = pagamento
                })
            };

            await _producer.ProduceAsync("pagamento-realizado", message);
        }

        public async Task SendPagamentoAtualizadoMessage(Pagamento pagamento)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(new
                {
                    Event = "PagamentoAtualizado",
                    Data = pagamento
                })
            };

            await _producer.ProduceAsync("pagamento-atualizado", message);
        }

        public async Task SendPagamentoExcluidoMessage(Pagamento pagamento)
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(new
                {
                    Event = "PagamentoExcluido",
                    Data = pagamento
                })
            };

            await _producer.ProduceAsync("pagamento-excluido", message);
        }
    }
}
