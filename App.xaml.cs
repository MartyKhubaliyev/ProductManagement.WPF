using ProductManagement.WPF.Services;
using ProductManagement.WPF.ViewModels;
using ProductManagement.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ProductManagement.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            base.OnStartup(e);

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Register services
            services.AddSingleton(sp =>
                new ApiService("https://localhost:7190/api/", "https://localhost:7064/api/"));
            services.AddSingleton(provider => new KafkaService("localhost:9092"));
            services.AddSingleton(provider => new MongoService("mongodb://localhost:27017", "Products"));

            // Register view models
            services.AddSingleton<CategoryViewModel>();
            services.AddSingleton<ProductViewModel>();
            services.AddSingleton<KafkaEventsViewModel>();

            // Register views
            services.AddSingleton<MainWindow>();
            services.AddTransient<CategoryView>();
            services.AddTransient<ProductView>();
            services.AddTransient<KafkaEventsView>();
        }
    }

}
