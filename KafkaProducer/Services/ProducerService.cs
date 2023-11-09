using Confluent.Kafka;
using System.Diagnostics;
using System.Net;

#nullable disable

namespace KafkaProducer.Services
{
    public class ProducerService : IProducerService
    {
        private string bootstrapServers;
        private IConfiguration configuration;
        private readonly ILogger<ProducerService> logger;

        public ProducerService(IConfiguration configuration, ILogger<ProducerService> logger)
        {
            this.configuration = configuration;
            bootstrapServers = configuration.GetSection("kafkaHost").Value;
            this.logger = logger;
        }

        public async Task<bool> ProduceAsync(string topicName, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName(),
            };


            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {

                    var result = await producer.ProduceAsync(topicName, new Message<Null, string>
                    {
                        Value = message,
                    });

                    logger.LogInformation($"Event with content of =>{message} Raised at => {result.Timestamp.UtcDateTime}");
                    return await Task.FromResult(true);


                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);




        }
    }
}
