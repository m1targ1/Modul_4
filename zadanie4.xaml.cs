using System.Collections.Generic;
using System.Windows;

namespace Modul_4
{
    public partial class zadanie4 : Window
    {
        private List<IBook> books; // Поле для хранения списка книг
        public zadanie4()
        {
            InitializeComponent();
            LoadBooks(); // Вызов метода для загрузки списка книг
        }                
        private void LoadBooks() // Метод для загрузки списка книг и добавления их в ListBox
        {
            // Создание списка книг с экземплярами FictionBook (художественная книга) и Textbook (учебник)
            books = new List<IBook>
            {
                new FictionBook("Война и мир", true),   // Художественная книга доступна
                new FictionBook("Анна Каренина", false),// Художественная книга недоступна
                new Textbook("Математика", true),       // Учебник доступен
                new Textbook("Физика", false)           // Учебник недоступен
            };
            // Добавление каждой книги в элемент ListBox, отображая тип книги и её название
            foreach (var book in books)
            {
                // Если книга — художественная, отображаем её как "Художественная" или как "Учебник"
                BooksList.Items.Add(book is FictionBook ?
                                   "Художественная: " + ((FictionBook)book).Title :
                                   "Учебник: " + ((Textbook)book).Title);
            }
        }        
        private void IssueBook_Click(object sender, RoutedEventArgs e) // Метод для обработки нажатия кнопки выдачи книги
        {
            // Проверка, выбрана ли книга в списке (индекс не должен быть равен -1)
            if (BooksList.SelectedIndex != -1)
            {
                // Получение выбранной книги на основе индекса в списке
                var selectedBook = books[BooksList.SelectedIndex];
                selectedBook.IssueBook(); // Выдача книги, с помощью метода IssueBook
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите книгу.");
            }
        }
        private void RefundBook_Click(object sender, RoutedEventArgs e) // Метод для обработки нажатия кнопки возврата книги
        {
            if (BooksList.SelectedIndex != -1) // Проверка, выбрана ли книга в списке
            {
                // Получение выбранной книги на основе индекса и вызов метода возврата
                var selectedBook = books[BooksList.SelectedIndex];
                selectedBook.RefundBook();
            }
            else
            {
                // Если книга не выбрана
                MessageBox.Show("Пожалуйста, выберите книгу.");
            }
        }
        public interface IBook // Интерфейс для всех типов книг. Определяет методы для проверки доступности, выдачи и возврата книг
        {
            bool IsAvailable(); // Метод для проверки, доступна ли книга
            void IssueBook();   // Метод для выдачи книги
            void RefundBook();  // Метод для возврата книги
        }              
        public class FictionBook : IBook // Класс художественных книг, реализующий интерфейс IBook
        {
            public string Title { get; private set; } // Свойство для названия книги
            private bool available; // Поле для хранения информации о доступности книги
                        
            public FictionBook(string title, bool isAvailable) // Конструктор, который задает название книги и её доступность
            {
                Title = title;
                available = isAvailable;
            }                       
            public bool IsAvailable() // Реализация метода проверки доступности книги
            {
                return available;
            }                      
            public void IssueBook() // Реализация метода для выдачи книги
            {
                // Если книга доступна, изменение её статуса и показ сообщение об успешной выдаче
                if (available)
                {
                    available = false; // Книга выдана, значит больше недоступна
                    MessageBox.Show($"Художественная книга \"{Title}\" выдана.");
                }
                else
                {
                    MessageBox.Show($"Художественная книга \"{Title}\" недоступна.");
                }
            }                    
            public void RefundBook() // Реализация метода для возврата книги
            {
                // Если книга выдана (недоступна), возврат её и она снова доступна
                if (!available)
                {
                    available = true;
                    MessageBox.Show($"Художественная книга \"{Title}\" возвращена.");
                }
                else
                {
                    MessageBox.Show($"Художественная книга \"{Title}\" уже доступна.");
                }
            }
        }
        public class Textbook : IBook // Класс учебников, реализующий интерфейс IBook
        {
            public string Title { get; private set; } // Свойство для названия учебника
            private bool available; // Поле для хранения информации о доступности учебника                    
            public Textbook(string title, bool isAvailable) // Конструктор, который задает название учебника и его доступность
            {
                Title = title;
                available = isAvailable;
            }                  
            public bool IsAvailable() // Реализация метода проверки доступности учебника
            {
                return available;
            }                 
            public void IssueBook() // Реализация метода для выдачи учебника
            {
                // Если учебник доступен, выдача его и он недоступен
                if (available)
                {
                    available = false;
                    MessageBox.Show($"Учебник \"{Title}\" выдан.");
                }
                else
                {
                    // Если учебник недоступен
                    MessageBox.Show($"Учебник \"{Title}\" недоступен.");
                }
            }
            public void RefundBook() // Реализация метода для возврата учебника
            {
                // Если учебник был выдан (недоступен), возврат его и он снова доступна
                if (!available)
                {
                    available = true;
                    MessageBox.Show($"Учебник \"{Title}\" возвращён.");
                }
                else
                {
                    // Если учебник уже доступен
                    MessageBox.Show($"Учебник \"{Title}\" уже доступен.");
                }
            }
        }      
    }
}
