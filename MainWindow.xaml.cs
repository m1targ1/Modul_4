using System;
using System.Windows;
using System.Windows.Controls;

namespace Mod_4
{
    // Интерфейс, представляющий общие методы для всех геометрических фигур
    public interface IShape
    {
        double GetArea(); // Метод для получения площади фигуры     
        double GetPerimeter(); // Метод для получения периметра фигуры
    }

    public class Circle : IShape // Класс, представляющий круг, который наследуется от интерфейса IShape
    {
        public double Radius { get; set; } // Свойство для хранения радиуса круга

        // Конструктор, принимающий радиус круга
        public Circle(double radius) => Radius = radius;
        public double GetArea() => Math.PI * Math.Pow(Radius, 2); // Реализация метода для расчета площади круга
        public double GetPerimeter() => 2 * Math.PI * Radius; // Реализация метода для расчета периметра круга
    }

    public class Rectangle : IShape // Класс, представляющий прямоугольник, наследуемый от интерфейса IShape
    {
        // Свойства для хранения ширины и высоты прямоугольника
        public double Width { get; set; }
        public double Height { get; set; }

        // Конструктор, принимающий ширину и высоту
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public double GetArea() => Width * Height;// Реализация метода для расчета площади прямоугольника
        public double GetPerimeter() => 2 * (Width + Height); // Реализация метода для расчета периметра прямоугольника
    }
    public class Triangle : IShape // Класс, представляющий треугольник, наследуемый от интерфейса IShape
    {
        // Свойства для хранения длин сторон треугольника
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        // Конструктор, принимающий длины сторон треугольника
        public Triangle(double a, double b, double c)
        {
            A = a; B = b; C = c;
        }

        // Метод для расчета площади треугольника
        public double GetArea()
        {
            double s = (A + B + C) / 2; // Полупериметр
            return Math.Sqrt(s * (s - A) * (s - B) * (s - C)); // Формула Герона
        }
        public double GetPerimeter() => A + B + C; // Реализация метода для расчета периметра треугольника
    }

    public partial class MainWindow : Window
    {
        // Поле для хранения текущей фигуры
        private IShape _currentShape;
        public MainWindow()
        {
            InitializeComponent();
        }

        // Метод, обрабатывающий изменение выбора в ComboBox (выбор фигуры)
        private void ShapeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputPanel.Children.Clear(); // Очистка панели ввода параметров

            // В зависимости от выбранного индекса ComboBox, создает текстовые поля для ввода параметров фигуры
            switch (ShapeComboBox.SelectedIndex)
            {
                case 0: // Круг
                    AddTextBox("Радиус", "RadiusTextBox");
                    break;
                case 1: // Прямоугольник
                    AddTextBox("Ширина", "WidthTextBox"); AddTextBox("Высота", "HeightTextBox");
                    break;
                case 2: // Треугольник
                    AddTextBox("Сторона A", "SideATextBox"); AddTextBox("Сторона B", "SideBTextBox"); AddTextBox("Сторона C", "SideCTextBox");
                    break;
            }
        }
        // Вспомогательный метод для добавления текстового поля ввода параметра фигуры
        private void AddTextBox(string labelText, string textBoxName)
        {
            StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal }; // Горизонтальный контейнер
            TextBlock label = new TextBlock { Text = labelText, Width = 100 }; // Текстовый блок для метки параметра           
            TextBox textBox = new TextBox { Name = textBoxName, Width = 100, Margin = new Thickness(10, 0, 0, 0) }; // Текстовое поле для ввода значения параметра

            // Добавление метки и текстового поля в контейнер
            panel.Children.Add(label);
            panel.Children.Add(textBox);

            InputPanel.Children.Add(panel); // Добавление контейнера в панель ввода
        }

        // Метод, обрабатывающий нажатие кнопки "Рассчитать"
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // В зависимости от выбранной фигуры создает соответствующий объект фигуры и передает параметры
                switch (ShapeComboBox.SelectedIndex)
                {
                    case 0: // Круг
                        double radius = double.Parse(((TextBox)((StackPanel)InputPanel.Children[0]).Children[1]).Text);
                        _currentShape = new Circle(radius);
                        break;
                    case 1: // Прямоугольник
                        double width = double.Parse(((TextBox)((StackPanel)InputPanel.Children[0]).Children[1]).Text);
                        double height = double.Parse(((TextBox)((StackPanel)InputPanel.Children[1]).Children[1]).Text);
                        _currentShape = new Rectangle(width, height);
                        break;
                    case 2: // Треугольник
                        double a = double.Parse(((TextBox)((StackPanel)InputPanel.Children[0]).Children[1]).Text);
                        double b = double.Parse(((TextBox)((StackPanel)InputPanel.Children[1]).Children[1]).Text);
                        double c = double.Parse(((TextBox)((StackPanel)InputPanel.Children[2]).Children[1]).Text);
                        _currentShape = new Triangle(a, b, c);
                        break;
                }
                // Вычисление площади и периметра фигуры
                double area = _currentShape.GetArea();
                double perimeter = _currentShape.GetPerimeter();

                ResultTextBlock.Text = $"Площадь: {area:F2}, Периметр: {perimeter:F2}"; // Вывод результатов в текстовый блок
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
