using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EKZ
{
    public partial class MainWindow : Window
    {
        private List<Product> allProducts;

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();

            SearchBox.TextChanged += (_, __) => ApplyFilters();
            PriceFrom.TextChanged += (_, __) => ApplyFilters();
            PriceTo.TextChanged += (_, __) => ApplyFilters();
            SortComboBox.SelectionChanged += (_, __) => ApplyFilters();
        }

        private void LoadProducts()
        {
            using (var db = new EKZEntities())
            {
                allProducts = db.Product.ToList();
                ProductsList.ItemsSource = allProducts;
            }
        }

        private void ApplyFilters()
        {
            if (allProducts == null) return;

            var filtered = allProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                filtered = filtered.Where(p =>
                    p.Name?.IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);

            if (decimal.TryParse(PriceFrom.Text, out decimal from))
                filtered = filtered.Where(p => p.Price >= from);
            if (decimal.TryParse(PriceTo.Text, out decimal to))
                filtered = filtered.Where(p => p.Price <= to);
            

            switch (SortComboBox.SelectedIndex)
            {
                case 1: filtered = filtered.OrderBy(p => p.Name); break;
                case 2: filtered = filtered.OrderByDescending(p => p.Name); break;
                case 3: filtered = filtered.OrderBy(p => p.Price); break;
                case 4: filtered = filtered.OrderByDescending(p => p.Price); break;
            }

            ProductsList.ItemsSource = filtered.ToList();
        }

        
            private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow { Owner = this };
            if (addWindow.ShowDialog() == true)
            {
                LoadProducts();   
                ApplyFilters();   
            }
        }
    
    }
}