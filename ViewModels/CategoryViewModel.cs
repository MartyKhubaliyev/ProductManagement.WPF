using MvvmHelpers;
using ProductManagement.WPF.Commands;
using ProductManagement.WPF.Helpers;
using ProductManagement.WPF.Models;
using ProductManagement.WPF.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ProductManagement.WPF.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();
        private Category _selectedCategory;
        private Category _newCategory;

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        public Category NewCategory
        {
            get => _newCategory;
            set
            {
                _newCategory = value;
                OnPropertyChanged();
                if (AddCommand != null)
                {
                    ((RelayCommand)AddCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public CategoryViewModel(ApiService apiService)
        {
            _apiService = apiService;

            Categories = new ObservableCollection<Category>();
            NewCategory = new Category();

            RefreshCommand = new RelayCommand(async () => await LoadCategoriesAsync());
            AddCommand = new RelayCommand(async () => await AddCategoryAsync(), CanAddCategory);
            EditCommand = new RelayCommand(async () => await EditCategoryAsync(), CanEditCategory);
            DeleteCommand = new RelayCommand(async () => await DeleteCategoryAsync(), CanDeleteCategory);
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

        private async Task AddCategoryAsync()
        {
            try
            {
                if (NewCategory != null && !string.IsNullOrWhiteSpace(NewCategory.Name))
                {
                    await _apiService.AddCategoryAsync(NewCategory);
                    NewCategory = new Category(); // Reset after adding
                    await LoadCategoriesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task EditCategoryAsync()
        {
            try
            {
                if (SelectedCategory != null && !string.IsNullOrWhiteSpace(NewCategory.Name))
                {
                    if (SelectedCategory.Name != NewCategory.Name)
                    {
                        SelectedCategory.Name = NewCategory.Name;
                        await _apiService.UpdateCategoryAsync(SelectedCategory);
                        await LoadCategoriesAsync();
                    }
                    else
                    {
                        ErrorDialogHelper.ShowErrorDialog($"Change Name of the category to update it!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task DeleteCategoryAsync()
        {
            try
            {
                if (SelectedCategory != null)
                {
                    await _apiService.DeleteCategoryAsync(SelectedCategory.Id);
                    await LoadCategoriesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool CanAddCategory()
        {
            return NewCategory != null && !string.IsNullOrWhiteSpace(NewCategory.Name);
        }

        private bool CanEditCategory()
        {
            return SelectedCategory != null;
        }

        private bool CanDeleteCategory()
        {
            return SelectedCategory != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
