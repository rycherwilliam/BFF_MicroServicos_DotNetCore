namespace BFFAPI.Application.Services.Mensageria
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string TopicCadastroClientes { get; set; }
        public string TopicCadastroPagamento { get; set; }
    }
}
