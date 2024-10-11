using System.Collections.Generic;
using System.Windows;

using static Modul_4.zadanie2;

namespace Modul_4
{
    public partial class zadanie2 : Window
    {
        private List<IProduct> _products;  // Поле класса для хранения списка товаров   
        public zadanie2()
        {
            InitializeComponent();
            LoadProducts();  // Загрузка товаров
            ProductList.ItemsSource = _products;  // Источник данных для ListBox
            ProductList.DisplayMemberPath = "Name";  // Отображение имен товаров
        }

        public interface IProduct // Интерфейс IProduct
        {
            int GetPrice();  // Метод для получения стоимости товара
            int GetStock();      // Метод для получения остатка на складе
            string GetName();    // Метод для получения названия товара
        }

        public class FoodProduct : IProduct // Класс для продуктов питания, реализующий интерфейс IProduct
        {
            public string Name { get; set; }
            public int PricePerUnit { get; set; }
            public int Stock { get; set; }

            public int GetPrice() => PricePerUnit;
            public int GetStock() => Stock;
            public string GetName() => Name;
        }
        
        public class ElectronicsProduct : IProduct // Класс для бытовой техники, реализующий интерфейс IProduct
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int Stock { get; set; }

            public int GetPrice() => Price;
            public int GetStock() => Stock;
            public string GetName() => Name;
        }
                
        private void LoadProducts() // Метод для загрузки списка товаров
        {
            _products = new List<IProduct>
            {
                new FoodProduct { Name = "Хлеб", PricePerUnit = 1, Stock = 50 },
                new FoodProduct { Name = "Молоко", PricePerUnit = 2, Stock = 30 },
                new ElectronicsProduct { Name = "Телевизор", Price = 2600, Stock = 10 },
                new ElectronicsProduct { Name = "Ноутбук", Price = 4100, Stock = 5 }
            };
        }
        
        private void ProductList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) // Обработчик события при изменении товара
        {
            if (ProductList.SelectedItem is IProduct selectedProduct)
            {
                ProductName.Text = selectedProduct.GetName();
                ProductPrice.Text = selectedProduct.GetPrice().ToString();
                ProductStock.Text = selectedProduct.GetStock().ToString();
            }
        }
    }
}

