using System;
using System.Linq;
using System.Windows;

namespace EKZ
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields(out string error))
            {
                MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var product = new Product
                {
                    Category = CategoryBox.Text.Trim(),
                    Name = NameBox.Text.Trim(),
                    Description = DescriptionBox.Text.Trim(),
                    Manufacturer = ManufacturerBox.Text.Trim(),
                    Supplier = SupplierBox.Text.Trim(),
                    Price = decimal.Parse(PriceBox.Text.Trim()),
                    Unit = UnitBox.Text.Trim(),
                    Quantity = int.Parse(QuantityBox.Text.Trim()),
                    Discount = int.Parse(DiscountBox.Text.Trim())
                };

                using (var db = new EKZEntities())
                {
                    db.Product.Add(product);
                    db.SaveChanges();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFields(out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(NameBox.Text))
            { error = "Введите название товара."; return false; }

            if (!decimal.TryParse(PriceBox.Text.Trim(), out decimal price) || price < 0)
            { error = "Цена должна быть неотрицательным числом."; return false; }

            if (!int.TryParse(QuantityBox.Text.Trim(), out int qty) || qty < 0)
            { error = "Количество должно быть целым неотрицательным числом."; return false; }

            if (!decimal.TryParse(DiscountBox.Text.Trim(), out decimal discount)
                || discount < 0 || discount > 100)
            { error = "Скидка должна быть числом от 0 до 100."; return false; }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}