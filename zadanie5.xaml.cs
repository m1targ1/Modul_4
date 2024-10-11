using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CombinedDrawingApp
{
    public partial class MainWindow : Window
    {
        private string _selectedShape = ""; // Установка значения по умолчанию для выбранной фигуры
        private Point _startPoint; // Начальная точка для рисования
        private bool _isDrawing; // Флаг, указывающий, что рисование начато
        private Shape? _currentShape; // Текущая рисуемая фигура
        private CanvasDrawer _canvasDrawer; // Объект для рисования на холсте
        private bool _isDrawingMode = true; // Флаг для состояния рисования

        public MainWindow()
        {
            InitializeComponent();
            _canvasDrawer = new CanvasDrawer(); // Инициализация класса для рисования
        }

        // Обработчики событий для кнопок выбора формы
        private void DrawLineButton_Click(object sender, RoutedEventArgs e) => _selectedShape = "Line";
        private void DrawCircleButton_Click(object sender, RoutedEventArgs e) => _selectedShape = "Circle";
        private void DrawRectangleButton_Click(object sender, RoutedEventArgs e) => _selectedShape = "Rectangle";

        // Обработчик события для кнопки очистки холста
        private void ClearCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Children.Clear(); // Очистка всех фигуры на холсте
            _isDrawingMode = true; // Возвращение в режим рисования после очистки
        }

        // Обработчик события нажатия мыши на холст
        private void DrawingCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawingMode) return; // Если не в режиме рисования, выход из метода

            _startPoint = e.GetPosition(DrawingCanvas); // Получение начальной точки от курсора мыши
            _isDrawing = true; // Установка флаг, что рисование начато

            // Инициализация текущей фигуры в зависимости от выбранного инструмента
            if (_selectedShape == "Line")
            {
                // Создание линии и добавление её на холст
                _currentShape = new Line
                {
                    Stroke = Brushes.Black, // Цвет линии
                    StrokeThickness = 2, // Толщина линии
                    X1 = _startPoint.X, // Начальная точка по оси X
                    Y1 = _startPoint.Y  // Начальная точка по оси Y
                };
                DrawingCanvas.Children.Add(_currentShape); // Добавление линию на холст
            }
            else if (_selectedShape == "Circle")
            {
                // Создание окружность и добавление её на холст
                _currentShape = new Ellipse
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                // Установка начальных координат окружности
                Canvas.SetLeft(_currentShape, _startPoint.X);
                Canvas.SetTop(_currentShape, _startPoint.Y);
                DrawingCanvas.Children.Add(_currentShape);
            }
            else if (_selectedShape == "Rectangle")
            {
                // Создание прямоугольника и добавление его на холст
                _currentShape = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                // Установка начальных координат прямоугольника
                Canvas.SetLeft(_currentShape, _startPoint.X);
                Canvas.SetTop(_currentShape, _startPoint.Y);
                DrawingCanvas.Children.Add(_currentShape);
            }
        }

        // Обработчик события движения мыши по холсту
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing && _currentShape != null) // Проверка, идет ли рисование
            {
                Point currentPoint = e.GetPosition(DrawingCanvas); // Получение текущей позиции курсора

                // Обновление размера фигуры в зависимости от положения мыши
                if (_selectedShape == "Line" && _currentShape is Line line)
                {
                    // Обновление координат конца линии
                    line.X2 = currentPoint.X;
                    line.Y2 = currentPoint.Y;
                }
                else if (_selectedShape == "Circle" && _currentShape is Ellipse ellipse)
                {
                    // Рассчёт радиуса и обновление размера окружности
                    double radius = Math.Sqrt(Math.Pow(currentPoint.X - _startPoint.X, 2) + Math.Pow(currentPoint.Y - _startPoint.Y, 2));
                    ellipse.Width = radius * 2;
                    ellipse.Height = radius * 2;
                    Canvas.SetLeft(ellipse, _startPoint.X - radius); // Установка положение окружности
                    Canvas.SetTop(ellipse, _startPoint.Y - radius);
                }
                else if (_selectedShape == "Rectangle" && _currentShape is Rectangle rectangle)
                {
                    // Рассчёт ширины и высоты, обновление размеров прямоугольника
                    double width = currentPoint.X - _startPoint.X;
                    double height = currentPoint.Y - _startPoint.Y;
                    rectangle.Width = Math.Abs(width); // Ширина
                    rectangle.Height = Math.Abs(height); // Высота
                    // Установка положения прямоугольника
                    Canvas.SetLeft(rectangle, width < 0 ? currentPoint.X : _startPoint.X);
                    Canvas.SetTop(rectangle, height < 0 ? currentPoint.Y : _startPoint.Y);
                }
            }
        }

        // Обработчик события отпускания кнопки мыши
        private void DrawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false; // Остановка рисование
            _currentShape = null; // Сброс текущую фигуру
            _isDrawingMode = true;
        }
    }

    // Интерфейс для рисуемых фигур
    public interface IDrawable
    {
        void DrawLine(Canvas canvas, Point startPoint, Point endPoint); // Метод для рисования линии
        void DrawCircle(Canvas canvas, Point center, double radius); // Метод для рисования окружности
        void DrawRectangle(Canvas canvas, Point topLeft, double width, double height); // Метод для рисования прямоугольника
    }

    // Класс для рисования фигур на холсте
    public class CanvasDrawer : IDrawable
    {
        public void DrawLine(Canvas canvas, Point startPoint, Point endPoint)
        {
            // Создание линии и добавление её на холст
            Line line = new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y
            };
            canvas.Children.Add(line); // Добавление линии на холст
        }

        public void DrawCircle(Canvas canvas, Point center, double radius)
        {
            // Создание окружности и добавление её на холст
            Ellipse ellipse = new Ellipse
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Width = radius * 2,
                Height = radius * 2
            };
            // Установка положения окружности
            Canvas.SetLeft(ellipse, center.X - radius);
            Canvas.SetTop(ellipse, center.Y - radius);
            canvas.Children.Add(ellipse); // Добавление окружности на холст
        }

        public void DrawRectangle(Canvas canvas, Point topLeft, double width, double height)
        {
            // Создание прямоугольника и добавление его на холст
            Rectangle rectangle = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Width = width,
                Height = height
            };
            // Установка положения прямоугольника
            Canvas.SetLeft(rectangle, topLeft.X);
            Canvas.SetTop(rectangle, topLeft.Y);
            canvas.Children.Add(rectangle); // Добавление прямоугольника на холст
        }
    }
}