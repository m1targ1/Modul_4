using System.Windows;

namespace Modul_4
{
    public partial class InputDialog : Window
    {
        public string InputText { get; private set; } // Публичное свойство для хранения текста, введенного пользователем

        // Конструктор, который инициализирует диалог с пользовательским сообщением (подсказкой)
        public InputDialog(string prompt)
        {
            InitializeComponent();
            PromptTextBlock.Text = prompt; // Устанавливает текст подсказки в соответствии с переданным аргументом
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия кнопки "OK"
        {
            InputText = InputTextBox.Text; // Сохраняет текст, введенный пользователем, в свойство InputText
            DialogResult = true; // Устанавливает результат диалога в true, указывая на успешный ввод (кнопка OK была нажата)
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) // Обработчик события нажатия кнопки "Отмена"
        {
            DialogResult = false; // Устанавливает результат диалога в false, указывая на отмену операции (кнопка "Отмена" была нажата)
        }
    }
}
