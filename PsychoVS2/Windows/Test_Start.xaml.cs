using Antlr.Runtime.Tree;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PsychoVS2.Windows
{
    /// <summary>
    /// Логика взаимодействия для Test_choice.xaml
    /// </summary>
    public partial class Test_Start : Window
    {

        private Psycho_Test choosen_one;
        private System.Windows.Controls.Label[] Author_labels;

        public Test_Start(Psycho_Test choosen_test)
        {
            this.choosen_one = choosen_test;
            InitializeComponent();
            WindowState = WindowState.Maximized;
            this.Author_labels = new System.Windows.Controls.Label[] { this.Author_label_1, this.Author_label_2, this.Author_label_3, this.Author_label_4 };
            this.Show_test_properties();
        }

        private void Show_test_properties()
        {
            this.Time_button.Content = $"{this.choosen_one.estemated_time} минут";
            this.Quastion_quantity.Content = this.choosen_one.amm_of_questions.ToString() + " Вопросов";
            if (this.choosen_one.type != "")
            {
                this.Test_type.Content = this.choosen_one.type;
            }
            else
                this.Test_type.Visibility = Visibility.Hidden;
            this.Name_label.Text = this.choosen_one.name;
            this.Description_label.Document.Blocks.Clear();
            this.Description_label.Document.Blocks.Add(new Paragraph(new Run(this.choosen_one.description)));
            string[] Authors = new string[4];
            this.choosen_one.author.Split('|').CopyTo(Authors, 0);
            for (int i = 0; i < 4; i++)
            {
                if (Authors[i] != null && Authors[i] != "")
                {
                    this.Author_labels[i].Content = Authors[i];
                }
                else 
                    this.Author_labels[i].Visibility = Visibility.Hidden;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Test_choice testChoiceWindow = new Test_choice();
            testChoiceWindow.Show();
            this.Close();
        }

        private void StartTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.choosen_one.amm_of_questions != 0)
            {
                Test Test = new Test(this.choosen_one);
                Test.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("In this test questions don`t exit. Stupid BD!!!", "0 questions", MessageBoxButton.OK, MessageBoxImage.Warning);
                Test_choice testChoiceWindow = new Test_choice();
                testChoiceWindow.Show();
                this.Close();
            }
            
        }

    }
}