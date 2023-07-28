using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BFFAPI.Application.Services.Mensageria
{
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _clienteTopic;
        private readonly string _pagamentoTopic;

        public KafkaProducerService(IProducer<Null, string> producer, IOptions<KafkaSettings> kafkaSettings)
        {
            _producer = producer;
            _clienteTopic = kafkaSettings.Value.TopicCadastroClientes;
            _pagamentoTopic = kafkaSettings.Value.TopicCadastroPagamento;
        }

        public async Task ProduceEventoCadastroCliente(EventoCadastroCliente eventoCadastroCliente)
        {
            var message = JsonConvert.SerializeObject(eventoCadastroCliente);
            var deliveryResult = await _producer.ProduceAsync(_clienteTopic, new Message<Null, string> { Value = message });
        }
        
        public async Task ProduceEventoCadastroPagamento(EventoCadastroPagamento eventoCadastroPagamento)
        {
            var message = JsonConvert.SerializeObject(eventoCadastroPagamento);
            var deliveryResult = await _producer.ProduceAsync(_pagamentoTopic, new Message<Null, string> { Value = message });
        }
    }
}

