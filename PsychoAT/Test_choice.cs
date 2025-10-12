using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PsychoAT
{
    public partial class Test_choice : Form
    {
        Guna.UI2.WinForms.Guna2Button[] Button_Array;
        Label[] Array_Type_labels;
        Label[] Array_Author_Labels;
        private Psycho_Test[] Tests_array;
        private Work_with_test_choice_page Test_choise_logic;
        public Test_choice()
        {
            InitializeComponent();
            this.Button_Array = new Guna.UI2.WinForms.Guna2Button[] { test_choise_1, test_choise_2, test_choise_3, test_choise_4, test_choise_5 };
            this.Array_Type_labels = new Label[] { TypeT1, TypeT2, TypeT3, TypeT4, TypeT5 };
            this.Array_Author_Labels = new Label[] { Author1, Author2, Author3, Author4, Author5 };
            this.Test_choise_logic = Program.Test_choise_logic;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Test_choice_Load(object sender, EventArgs e)
        {
            this.Show_tests_on_page(this.Test_choise_logic.Test_for_show_before_load());
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.w_Start.Show();
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Test_choice_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void test_choise_1_Click(object sender, EventArgs e)
        {
            this.Go_to_main_test_window(0);
        }

        private void test_choise_2_Click(object sender, EventArgs e)
        {
            this.Go_to_main_test_window(1);
        }

        private void test_choise_3_Click(object sender, EventArgs e)
        {
            this.Go_to_main_test_window(2);
        }

        private void test_choise_4_Click(object sender, EventArgs e)
        {
            this.Go_to_main_test_window(3);
        }

        private void test_choise_5_Click(object sender, EventArgs e)
        {
            this.Go_to_main_test_window(4);
        }

        private void Go_to_main_test_window(short index_of_choosen_test)
        {
            Program.Main_test_page.Set_current_test(this.Tests_array[index_of_choosen_test]);
            Program.w_Test_Choice.Hide();
            Program.w_Test_Start.Show();
        }

        private void test_page_next_Click(object sender, EventArgs e)
        {
            this.Show_tests_on_page(this.Test_choise_logic.Get_on_next_page());
        }
        private void test_page_back_Click(object sender, EventArgs e)
        {
            this.Show_tests_on_page(this.Test_choise_logic.Get_on_previous_page());
        }
        private void Show_tests_on_page(Psycho_Test[] Array_of_tests)
        {
            this.Tests_array = Array_of_tests;
            short size_of_tests_array = (short)this.Tests_array.Length;
            for (short i = 0; i < size_of_tests_array; i++)
            {
                if (!(Array_of_tests[i] is null))
                    this.Change_text_of_a_button(this.Tests_array[i], i);
                else
                    Hide_unused_buttons_and_labels(i);
            }
            this.Bottom_buttons_logic();
        }
        private void Change_text_of_a_button(Psycho_Test Test_that_needs_to_be_displayed, short index_in_array)
        {
            this.Button_Array[index_in_array].Text = Test_that_needs_to_be_displayed.name;
            this.Button_Array[index_in_array].Visible = true;
            this.Button_Array[index_in_array].Enabled = true;
            this.Array_Type_labels[index_in_array].Text = Test_that_needs_to_be_displayed.type;
            this.Array_Type_labels[index_in_array].Visible = true;
            this.Array_Author_Labels[index_in_array].Text = Test_that_needs_to_be_displayed.author;
            this.Array_Author_Labels[index_in_array].Visible = true;
        }
        private void Hide_unused_buttons_and_labels(short index_of_button_and_labels)
        {
            this.Button_Array[index_of_button_and_labels].Visible = false;
            this.Button_Array[index_of_button_and_labels].Enabled = false;
            this.Array_Type_labels[index_of_button_and_labels].Visible = false;
            this.Array_Author_Labels[index_of_button_and_labels].Visible = false;
        }
        private void Bottom_buttons_logic()
        {
            if (this.Test_choise_logic.Is_it_only_page())
            {
                this.test_page_back.Enabled = false;
                this.test_page_next.Enabled = false;
                return;
            }
            if (this.Test_choise_logic.Is_page_last())
            {
                this.test_page_next.Enabled = false;
                this.test_page_back.Enabled = true;
                return;
            }
            else if (this.Test_choise_logic.Is_page_first())
            {
                this.test_page_back.Enabled = false;
                this.test_page_next.Enabled = true;
                return;
            }
            this.test_page_back.Enabled = true;
            this.test_page_next.Enabled = true;
        }
    }
}
