using System.Windows;
using System.Windows.Controls;

namespace PsychoVS2.Windows
{
    public partial class Test : Window
    {
        private Button selectedAnswer;

        public Test()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            // Снимаем выделение с предыдущего выбранного ответа
            if (selectedAnswer != null)
            {
                selectedAnswer.Style = (Style)FindResource("AnswerButtonStyle");
            }

            // Выделяем новый выбранный ответ
            selectedAnswer = (Button)sender;
            selectedAnswer.Style = (Style)FindResource("SelectedAnswerButtonStyle");

            // Активируем кнопку "Далее"
            NextButton.IsEnabled = true;
            SelectionIndicator.Content = "✓ Ответ выбран";
            SelectionIndicator.Foreground = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromArgb(0xFF, 0xFC, 0xCC, 0x3C));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Навигация назад
            MessageBox.Show("Переход к предыдущему вопросу");
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Result result = new Result();
            result.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из теста
            var result = MessageBox.Show("Вы уверены, что хотите выйти из теста?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Test_Start testStartWindow = new Test_Start();
                testStartWindow.Show();
                this.Close();
            }
        }
    }
}