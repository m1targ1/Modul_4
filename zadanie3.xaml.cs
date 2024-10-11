using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modul_4
{
    public partial class zadanie3 : Window
    {
        private Dictionary<string, List<Student>> _courses; // Словарь для хранения курсов и соответствующих студентов
        private List<string> _subjects; // Список предметов

        public zadanie3()
        {
            InitializeComponent();
            InitializeData(); // Вызов метода для инициализации данных
            LoadSubjects(); // Вызов метода для загрузки предметов
        }
        private void InitializeData() // Метод для инициализации данных
        {
            // Инициализация словаря с курсами и списками студентов
            _courses = new Dictionary<string, List<Student>>()
            {
                { "1 курс", new List<Student> { new Student("Иванов Иван", "1 курс") } }, // 1 курс
                { "2 курс", new List<Student> { new Student("Петров Петр", "2 курс") } }, // 2 курс
                { "3 курс", new List<Student> { new Student("Сидоров Сидор", "3 курс") } } // 3 курс
            };
        }
        private void LoadSubjects() // Метод для загрузки предметов
        {
            // Предзаданные предметы
            _subjects = new List<string> { "Математика", "Иностранный язык", "История", "Физика", "Химия" };
            SubjectsComboBox.ItemsSource = _subjects; // Установка источника данных для ComboBox предметов
            SubjectsComboBox.IsEnabled = true; // Включение ComboBox
        }
        private void CoursesListBox_SelectionChanged(object sender, RoutedEventArgs e) // Обработчик изменения выбора в списке курсов
        {
            if (CoursesListBox.SelectedItem != null) // Проверка, что выбран элемент
            {
                string selectedCourse = (CoursesListBox.SelectedItem as ListBoxItem)?.Content.ToString(); // Получение выбранного курса
                if (_courses.ContainsKey(selectedCourse)) // Проверка наличия курса в словаре
                {
                    StudentsListBox.ItemsSource = _courses[selectedCourse]; // Установка источника данных для списка студентов
                    StudentsListBox.DisplayMemberPath = "Name"; // Установка свойства для отображения имени студента
                }
            }
        }
        private void StudentsListBox_SelectionChanged(object sender, RoutedEventArgs e) // Обработчик изменения выбора в списке студентов
        {
            if (StudentsListBox.SelectedItem is Student selectedStudent) // Проверка, что выбран студент
            {
                // Получение данных оценок для выбранного студента
                var gradesData = selectedStudent.Grades.Select(g => new
                {
                    Subject = g.Key, // Название предмета
                    Grades = string.Join(",", g.Value), // Оценки, представленные в виде строки
                    Average = g.Value.Any() ? g.Value.Average() : 0 // Средняя оценка или 0, если оценок нет
                }).ToList();
                GradesDataGrid.ItemsSource = gradesData; // Установка источника данных для таблицы оценок
                AverageGradeTextBlock.Text = $"Общий средний балл: {selectedStudent.GetAverageGrade():F2}"; // Отображение среднего балла
            }
        }
        private void AddStudent_Click(object sender, RoutedEventArgs e) // Обработчик нажатия кнопки добавления студента
        {
            string selectedCourse = (CoursesListBox.SelectedItem as ListBoxItem)?.Content.ToString(); // Получение выбранного курса
            if (!string.IsNullOrEmpty(selectedCourse) && _courses.ContainsKey(selectedCourse)) // Проверка, что курс выбран
            {
                var inputDialog = new InputDialog("Введите имя студента:"); // Создание диалогового окна для ввода имени
                if (inputDialog.ShowDialog() == true) // Проверка, что диалоговое окно закрыто с результатом "OK"
                {
                    string studentName = inputDialog.InputText; // Получение введенного имени
                    if (!string.IsNullOrEmpty(studentName)) // Проверка, что имя не пустое
                    {
                        var newStudent = new Student(studentName, selectedCourse); // Создание нового студента
                        _courses[selectedCourse].Add(newStudent); // Добавление студента в курс
                        StudentsListBox.Items.Refresh(); // Обновление списка студентов
                    }
                }
            }
        }
        private void RemoveStudent_Click(object sender, RoutedEventArgs e) // Обработчик нажатия кнопки удаления студента
        {
            if (StudentsListBox.SelectedItem is Student selectedStudent) // Проверка, что выбран студент
            {
                string selectedCourse = selectedStudent.Course; // Получение курса выбранного студента
                _courses[selectedCourse].Remove(selectedStudent); // Удаление студента из курса
                StudentsListBox.Items.Refresh(); // Обновление списка студентов
                GradesDataGrid.ItemsSource = null; // Очистка таблицы с оценками
                AverageGradeTextBlock.Text = string.Empty; // Очистка текста со средним баллом
            }
        }
        private void UpdateGrade_Click(object sender, RoutedEventArgs e) // Обработчик нажатия кнопки обновления оценок
        {
            if (StudentsListBox.SelectedItem is Student selectedStudent && SubjectsComboBox.SelectedItem is string subject) // Проверка выбора студента и предмета
            {
                var gradeDialog = new InputDialog("Введите новые оценки через запятую:"); // Создание диалогового окна для ввода оценок
                if (gradeDialog.ShowDialog() == true) // Проверка, что диалоговое окно закрыто с результатом "OK"
                {
                    string gradeStr = gradeDialog.InputText; // Получение введенных оценок
                    if (!string.IsNullOrEmpty(gradeStr)) // Проверка, что оценки не пустые
                    {
                        // Преобразование строковых оценок в список целых чисел
                        List<int> newGrades = gradeStr.Split(',')
                                                       .Select(g => int.TryParse(g.Trim(), out int grade) ? grade : 0) // Преобразование каждой оценки
                                                       .ToList();

                        selectedStudent.Grades[subject] = newGrades; // Обновление оценок для выбранного предмета
                        StudentsListBox_SelectionChanged(null, null); // Обновление отображаемых данных
                    }
                }
            }
            else // Если не выбран студент или предмет
            {
                MessageBox.Show("Сначала выберите студента и предмет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Сообщение об ошибке
            }
        }
        private void AddGrade_Click(object sender, RoutedEventArgs e) // Обработчик нажатия кнопки добавления оценок
        {
            if (StudentsListBox.SelectedItem is Student selectedStudent && SubjectsComboBox.SelectedItem is string subject) // Проверка выбора студента и предмета
            {
                var gradeDialog = new InputDialog("Введите оценки через запятую:"); // Создание диалогового окна для ввода оценок
                if (gradeDialog.ShowDialog() == true) // Проверка, что диалоговое окно закрыто с результатом "OK"
                {
                    string gradeStr = gradeDialog.InputText; // Получение введенных оценок
                    if (!string.IsNullOrEmpty(gradeStr)) // Проверка, что оценки не пустые
                    {
                        // Преобразование строковых оценок в список целых чисел
                        List<int> newGrades = gradeStr.Split(',')
                                                       .Select(g => int.TryParse(g.Trim(), out int grade) ? grade : 0) // Преобразование каждой оценки
                                                       .ToList();

                        // Проверка, существует ли предмет в оценках студента
                        if (!selectedStudent.Grades.ContainsKey(subject))
                        {
                            selectedStudent.Grades[subject] = new List<int>(); // Если не существует, создание нового списка оценок
                        }

                        selectedStudent.Grades[subject].AddRange(newGrades); // Добавление новых оценок к существующим
                        StudentsListBox_SelectionChanged(null, null); // Обновление отображаемых данных
                    }
                }
            }
            else // Если не выбран студент или предмет
            {
                MessageBox.Show("Сначала выберите студента и предмет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); // Сообщение об ошибке
            }
        }
    }
    public interface IStudent // Определение интерфейса IStudent
    {        
        string Name { get; set; } // Свойство для получения и установки имени студента            
        string Course { get; set; } // Свойство для получения и установки курса студента

        // Свойство для получения и установки оценок студента, где ключ — это название предмета, 
        // а значение — список оценок
        Dictionary<string, List<int>> Grades { get; set; }                
        double GetAverageGrade(); // Метод для вычисления средней оценки студента        
        string GetCourseInfo(); // Метод для получения информации о курсе студента
    }   
    public class Student : IStudent // Реализация интерфейса IStudent в классе Student
    {        
        public string Name { get; set; } // Реализация свойства Name для получения и установки имени студента      
        public string Course { get; set; } // Реализация свойства Course для получения и установки курса студента      
        public Dictionary<string, List<int>> Grades { get; set; } // Реализация свойства Grades для получения и установки оценок студента
        public Student(string name, string course) // Конструктор класса Student, который принимает имя и курс студента
        {
            Name = name; // Установка имени студента
            Course = course; // Установка курса студента
            Grades = new Dictionary<string, List<int>>(); // Инициализация словаря для оценок
        }        
        public double GetAverageGrade() // Метод для вычисления средней оценки студента
        {
            // Проверка, есть ли хоть одна оценка
            if (Grades.SelectMany(g => g.Value).Any())
            {
                // Если оценки есть, вычисляет и возвращает их среднее значение
                return Grades.SelectMany(g => g.Value).Average();
            }
            else
            {
                return 0; // Возвращает 0, если оценок нет
            }
        }      
        public string GetCourseInfo() // Метод для получения информации о курсе студента
        {
            // Формирование строки с информацией о студенте и его курсе
            return $"{Name} учится на курсе {Course}";
        }
    }
}
