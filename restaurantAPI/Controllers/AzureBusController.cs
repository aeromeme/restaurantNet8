using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.Application.Services;
using System.Text.Json;

namespace restaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureBusController : ControllerBase
    {
        private readonly ILogger<AzureBusController> _logger;
        private readonly ServiceBusService _serviceBus;
        private readonly string topicName = "topic.1";
        private string connectionString = "Endpoint=sb://localhost/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MyLocalSasKey123!;UseDevelopmentEmulator=true;";
        private string queueName = "queue.1";
        private string subscriptionName = "subscription.1";
        public AzureBusController(ILogger<AzureBusController> logger)
        {
            _logger = logger;
            // Use your local emulator connection string
            
            _serviceBus = new ServiceBusService(connectionString, queueName);
        }
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] string message)
        {
            
            await _serviceBus.SendMessageAsync(message);
            return Ok("Message sent!");
        }

        [HttpGet("receive")]
        public async Task<IActionResult> Receive()
        {
            var message = await _serviceBus.ReceiveMessageAsync();
            if (message == null)
                return NotFound("No messages in queue.");

            return Ok(message);
        }

        [HttpPost("topic")]
        public async Task<IActionResult> TopicSend([FromBody] string payload)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicName);

            var json = JsonSerializer.Serialize(payload);

            var message = new ServiceBusMessage(json)
            {
                MessageId = "msgid1",
                Subject = "subject1",
                CorrelationId = "id1",
                ContentType = "application/text",
                ReplyTo = "someQueue",
                SessionId = "session1",
                ReplyToSessionId = "sessionId",   // <- THIS WAS MISSING
                To = "xyz"
            };

            // add custom user property
            message.ApplicationProperties["prop3"] = "value3";

            await sender.SendMessageAsync(message);
            return Ok("Message sent to topic.");

        }

        // GET api/servicebus/receive/{subscription}
        [HttpGet("receive/{subscription}")]
        public async Task<IActionResult> ReceiveMessages(string subscription)
        {
            await using var client = new ServiceBusClient(connectionString);

            ServiceBusReceiver receiver = client.CreateReceiver(topicName, subscriptionName);

            var messages = await receiver.ReceiveMessagesAsync(maxMessages: 5, maxWaitTime: TimeSpan.FromSeconds(3));

            if (messages.Count == 0)
                return NotFound("No messages available.");

            //var results = messages.Select(m => new
            //{
            //    m.MessageId,
            //    Body = m.Body.ToString(),
            //    m.CorrelationId,
            //    Properties = m.ApplicationProperties
            //});

            var results = new List<object>();

            foreach (var msg in messages)
            {
                try
                {
                    // ----- PROCESS MESSAGE -----
                    var body = msg.Body.ToString();
                    Console.WriteLine($"Processing message: {body}");
                    //throw new Exception("Simulated processing error");
                    // Simulate processing logic
                    // Throw exception if processing fails
                    // Example: if (body.Contains("error")) throw new Exception("Processing failed");

                    // ----- COMPLETE -----
                    await receiver.CompleteMessageAsync(msg);

                    results.Add(new
                    {
                        msg.MessageId,
                        Body = body,
                        msg.CorrelationId,
                        Status = "Processed successfully"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");

                    // Option 1: Abandon → make message available again for retry
                    await receiver.AbandonMessageAsync(msg);

                    // Option 2: Dead-letter → move to subscription's DLQ
                    // await receiver.DeadLetterMessageAsync(msg, "Processing failed", ex.Message);

                    results.Add(new
                    {
                        msg.MessageId,
                        Body = msg.Body.ToString(),
                        msg.CorrelationId,
                        Status = "Failed, sent back for retry"
                    });
                }
            }


            return Ok(results);
        }

    }
}
