using ProductManagement.WPF.Commands;
using ProductManagement.WPF.Helpers;
using ProductManagement.WPF.Models;
using ProductManagement.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ProductManagement.WPF.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private Category _selectedCategory;
        private Product _selectedProduct;
        private Product _newProduct;

        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<Category> Categories { get; }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        public Product NewProduct
        {
            get => _newProduct;
            set
            {
                _newProduct = value;
                OnPropertyChanged();
                if (AddCommand != null)
                {
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }


        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProductViewModel(ApiService apiService)
        {
            _apiService = apiService;

            Products = new ObservableCollection<Product>();
            Categories = new ObservableCollection<Category>();
            NewProduct = new Product();

            RefreshCommand = new RelayCommand(async () => await LoadDataAsync());
            AddCommand = new RelayCommand(async () => await AddProductAsync(), CanAddProduct);
            EditCommand = new RelayCommand(async () => await EditProductAsync(), CanEditProduct);
            DeleteCommand = new RelayCommand(async () => await DeleteProductAsync(), CanDeleteProduct);
        }

        private async Task LoadDataAsync()
        {
            await LoadCategoriesAsync();
            await LoadProductsAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                Categories.Clear();
                var categories = await _apiService.GetCategoriesAsync();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                Products.Clear();
                var products = await _apiService.GetProductsAsync();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task AddProductAsync()
        {
            try
            {
                if (NewProduct != null && SelectedCategory != null)
                {
                    NewProduct.Category = SelectedCategory.Id;
                    await _apiService.AddProductAsync(NewProduct);
                    NewProduct = new Product(); // Reset after adding
                    await LoadProductsAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task EditProductAsync()
        {
            try
            {
                if (SelectedProduct != null)
                {
                    bool changed = false;
                    if (SelectedCategory != null && SelectedProduct.Category != SelectedCategory.Id)
                    {
                        SelectedProduct.Category = SelectedCategory.Id;
                        changed = true;
                    }
                    if (!string.IsNullOrWhiteSpace(NewProduct.Name) && SelectedProduct.Name != NewProduct.Name)
                    {
                        SelectedProduct.Name = NewProduct.Name;
                        changed = true;
                    }
                    if (changed)
                    {
                        await _apiService.UpdateProductAsync(SelectedProduct);
                        await LoadProductsAsync();
                    }
                    else
                    {
                        ErrorDialogHelper.ShowErrorDialog($"Change Category or Name of the product to update it!");
                    }
                    NewProduct = new Product();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task DeleteProductAsync()
        {
            try
            {
                if (SelectedProduct != null)
                {
                    await _apiService.DeleteProductAsync(SelectedProduct.Id);
                    await LoadProductsAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CanAddProduct()
        {
            return NewProduct != null && !string.IsNullOrWhiteSpace(NewProduct.Name) && SelectedCategory != null;
        }

        private bool CanEditProduct()
        {
            return SelectedProduct != null;
        }

        private bool CanDeleteProduct()
        {
            return SelectedProduct != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
