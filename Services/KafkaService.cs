using Confluent.Kafka;
using ProductManagement.WPF.Helpers;
namespace ProductManagement.WPF.Services
{
    public class KafkaService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        public event Action<string, string> OnMessageConsumed;

        public KafkaService(string kafkaBootstrapServers)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaBootstrapServers,
                GroupId = "wpf-app-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public void StartConsuming(string categoryTopic, string productTopic)
        {
            Task.Run(() =>
            {
                _consumer.Subscribe(new[] { categoryTopic, productTopic });
                while (true)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(CancellationToken.None);
                        var topic = consumeResult.Topic;
                        var message = consumeResult.Message.Value;

                        OnMessageConsumed?.Invoke(topic, message); // Notify subscribers
                    }
                    catch (ConsumeException e)
                    {
                        ErrorDialogHelper.ShowErrorDialog($"Error: {e.Error.Reason}");
                    }
                }
            });
        }

        public void StopConsuming()
        {
            _consumer.Close();
        }
    }
}
