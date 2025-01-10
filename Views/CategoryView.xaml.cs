using Microsoft.Extensions.DependencyInjection;
using ProductManagement.WPF.ViewModels;
using System.Windows.Controls;

namespace ProductManagement.WPF.Views
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<CategoryViewModel>();
        }
    }
}
