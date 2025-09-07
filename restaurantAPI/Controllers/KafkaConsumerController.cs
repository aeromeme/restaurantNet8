using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaConsumerController : ControllerBase
    {
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "my-topic";
        private  string _groupId = "restaurant-api-consumer-group";

        [HttpGet("consume")]
        public async Task<IActionResult> Consume([FromQuery] string topic, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(topic))
                return BadRequest("Topic is required.");
            _groupId = "api-group";
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);

            try
            {
                var timeout = TimeSpan.FromSeconds(5);
                var start = DateTime.UtcNow;
                while (!cancellationToken.IsCancellationRequested && DateTime.UtcNow - start < timeout)
                {
                    var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(3000));
                    if (consumeResult != null)
                    {
                        return Ok(new
                        {
                            Message = consumeResult.Message.Value,
                            Partition = consumeResult.Partition.Value,
                            Offset = consumeResult.Offset.Value
                        });
                    }
                }
                if (cancellationToken.IsCancellationRequested)
                    return StatusCode(499, "Request cancelled.");
                return NotFound("No message received.");
            }
            catch (ConsumeException ex)
            {
                return StatusCode(500, $"Kafka consume error: {ex.Error.Reason}");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}