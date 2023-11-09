using Confluent.Kafka;
using KafkaAgg;
using KafkaAgg.Events;
using KafkaConsumer.Extensions;
using Org.BouncyCastle.Utilities;
using System.Diagnostics;
using System.Text.Json;

#nullable disable

namespace KafkaConsumer
{
    public class KafkaConsumer : IHostedService
    {
        private IConfiguration _configuration;
        private readonly ILogger<KafkaConsumer> logger;

        public KafkaConsumer(IConfiguration configuration, ILogger<KafkaConsumer> logger)
        {
            _configuration = configuration;
            this.logger = logger;
        }



        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ConsumerConfig config = new ConsumerConfig()
            {
                GroupId = "MyCustomer",
                BootstrapServers = _configuration.GetSection("kafkaHost").Value,
                //AutoOffsetReset = AutoOffsetReset.,
                //EnableAutoCommit = false,
            };


            using (var c = new Confluent.Kafka.ConsumerBuilder<Null, string>(config).Build())
            {
                c.Subscribe(TopicName.WithdrawRequest.Name());

                CancellationTokenSource cts = new CancellationTokenSource();

                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };



                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            logger.LogInformation($"Consumed message with content of ==>>{cr.Value}");
                        }
                        catch (ConsumeException e)
                        {
                            logger.LogInformation($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
                //while (true)
                //{
                //    try
                //    {
                //        var cr = consumer.Consume();
                //        var message = cr.Message.Value.ToString();
                //        var MyModel = JsonSerializer.Deserialize<WithdrawRequestEvent>(message);
                //        Debug.WriteLine(MyModel.WalletId);
                //    }
                //    catch (Exception ex)
                //    {

                //        throw;
                //    }

                //}
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
