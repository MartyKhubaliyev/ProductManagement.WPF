using ProductManagement.WPF.Models;
using ProductManagement.WPF.Helpers;
using System.Net.Http;
using System.Net.Http.Json;

namespace ProductManagement.WPF.Services
{
    public class ApiService
    {
        private readonly HttpClient _categoryClient;
        private readonly HttpClient _productClient;

        public ApiService(string categoryApiBaseUrl, string productApiBaseUrl)
        {
            _categoryClient = new HttpClient { BaseAddress = new Uri(categoryApiBaseUrl) };
            _productClient = new HttpClient { BaseAddress = new Uri(productApiBaseUrl) };
        }

        #region Category

        // Category CRUD operations

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _categoryClient.GetFromJsonAsync<List<Category>>("Category")
                    ?? new List<Category>();
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error fetching categories: {ex.Message}");
                return new List<Category>();
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                await _categoryClient.PostAsJsonAsync("Category", category);
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error adding category: {ex.Message}");
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                var response = await _categoryClient.PutAsJsonAsync("Category", category);
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error updating category: {ex.Message}");
            }
        }

        public async Task DeleteCategoryAsync(long id)
        {
            try
            {
                await _categoryClient.DeleteAsync($"Category/{id}");
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error deleting categories: {ex.Message}");
            }
        }

        #endregion

        #region Products

        // Product CRUD operations

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                return await _productClient.GetFromJsonAsync<List<Product>>("Product")
                    ?? new List<Product>();
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error fetching products: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _productClient.PostAsJsonAsync("Product", product);
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error adding product: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                await _productClient.PutAsJsonAsync("Product", product);
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error updating product: {ex.Message}");
            }
        }

        public async Task DeleteProductAsync(long id)
        {
            try
            {
                await _productClient.DeleteAsync($"Product/{id}");
            }
            catch (Exception ex)
            {
                ErrorDialogHelper.ShowErrorDialog($"Error deleting product: {ex.Message}");
            }
        }

        #endregion
    }
}
