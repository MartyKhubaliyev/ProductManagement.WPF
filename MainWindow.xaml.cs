using System.Windows;

namespace ProductManagement.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (CategoryFrame.NavigationService.Content == null)
            {
                CategoryFrame.Navigate(new Views.CategoryView());
                ProductFrame.Navigate(new Views.ProductView());
                KafkaEventsFrame.Navigate(new Views.KafkaEventsView());
            }
        }
    }
}