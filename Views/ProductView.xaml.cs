using Microsoft.Extensions.DependencyInjection;
using ProductManagement.WPF.ViewModels;
using System.Windows.Controls;

namespace ProductManagement.WPF.Views
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : UserControl
    {
        public ProductView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<ProductViewModel>();
        }
    }
}
