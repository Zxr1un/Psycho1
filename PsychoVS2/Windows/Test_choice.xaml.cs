using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PsychoVS2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Test_Start.xaml
    /// </summary>
    public partial class Test_choice : Window
    {
        public Test_choice()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_One_Click(object sender, RoutedEventArgs e)
        {
            Test_Start testStartWindow = new Test_Start();
            testStartWindow.Show();
            this.Close();
        }
    }
}
