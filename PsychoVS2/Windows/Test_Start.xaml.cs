using System.Windows;

namespace PsychoVS2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Test_choice.xaml
    /// </summary>
    public partial class Test_Start : Window
    {
        public Test_Start()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Test_choice testChoiceWindow = new Test_choice();
            testChoiceWindow.Show();
            this.Close();
        }

        private void StartTestButton_Click(object sender, RoutedEventArgs e)
        {
            
            Test Test = new Test();
            Test.Show();
            this.Close();
            
        }

    }
}