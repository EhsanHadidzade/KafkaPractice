using KafkaAgg;
using KafkaAgg.Events;
using KafkaProducer.Extensions;
using KafkaProducer.Infrastructure.Mapper;
using KafkaProducer.models.DTOs;
using KafkaProducer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KafkaProducer.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WalletController : ControllerBase
    {

        private readonly ILogger<WalletController> _logger;
        private readonly IProducerService _producerService;

        public WalletController(ILogger<WalletController> logger, IProducerService producerService)
        {
            _logger = logger;
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<bool> WithdrawRequest(WithdrawRequestDTO reqeust)
        {
            var withdrawEvent = Mapper.Map<WithdrawRequestDTO, WithdrawRequestEvent>(reqeust);
            var message = JsonConvert.SerializeObject(withdrawEvent);
            var isSent = await _producerService.ProduceAsync(TopicName.WithdrawRequest.Name(), message);
            return isSent;
        }
    }
}