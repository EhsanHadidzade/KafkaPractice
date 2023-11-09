namespace KafkaProducer.Services
{
    public interface IProducerService
    {
        Task<bool> ProduceAsync(string topicName, string message);
    }
}
