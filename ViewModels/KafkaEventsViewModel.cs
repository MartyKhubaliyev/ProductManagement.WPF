using ProductManagement.WPF.Commands;
using ProductManagement.WPF.Enums;
using ProductManagement.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace ProductManagement.WPF.ViewModels
{
    public class KafkaEventsViewModel : INotifyPropertyChanged
    {
        private readonly KafkaService _kafkaService;
        private readonly MongoService _mongoService;
        private readonly Dispatcher _dispatcher;

        public ObservableCollection<string> KafkaMessages { get; } = new ObservableCollection<string>();


        public ICommand StartConsumingCommand { get; }
        public ICommand StopConsumingCommand { get; }
        public ICommand ClearEventsCommand { get; }

        public KafkaEventsViewModel(KafkaService kafkaService, MongoService mongoService)
        {
            _kafkaService = kafkaService;
            _mongoService = mongoService;
            _dispatcher = Dispatcher.CurrentDispatcher;

            _kafkaService.OnMessageConsumed += OnMessageConsumed;
            _kafkaService.StartConsuming("category-events", "product-events");
        }

        private async void OnMessageConsumed(string topic, string message)
        {
            // Add the message to the collection (which binds to the UI)
            _dispatcher.Invoke(() =>
            {
                KafkaMessages.Add($"Topic: {topic}, Message: {message}");
            });
            

            // Store the message in MongoDB
            if (topic == "category-events")
            {
                await _mongoService.AddCategoryEventAsync(topic, message);
            }
            else
            {
                await _mongoService.AddProductEventAsync(topic, message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Notify the UI when properties change
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
