using Azure.Messaging.ServiceBus;

namespace restaurantAPI.Application.Services
{
    public class ServiceBusService
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public ServiceBusService(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        public async Task SendMessageAsync(string message)
        {
            await using var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(_queueName);
            await sender.SendMessageAsync(new ServiceBusMessage(message));
        }

        public async Task<string> ReceiveMessageAsync()
        {
            await using var client = new ServiceBusClient(_connectionString);
            var receiver = client.CreateReceiver(_queueName);

            var message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
            if (message != null)
            {
                await receiver.CompleteMessageAsync(message);
                return message.Body.ToString();
            }

            return null;
        }
    }
}
