using Microsoft.Extensions.DependencyInjection;
using ProductManagement.WPF.ViewModels;
using System.Windows.Controls;

namespace ProductManagement.WPF.Views
{
    /// <summary>
    /// Interaction logic for KafkaEventsView.xaml
    /// </summary>
    public partial class KafkaEventsView : UserControl
    {
        public KafkaEventsView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<KafkaEventsViewModel>();
        }
    }
}
