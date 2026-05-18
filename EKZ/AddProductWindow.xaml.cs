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
    }
}