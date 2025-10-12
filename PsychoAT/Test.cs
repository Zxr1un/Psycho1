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
    public partial class Test : Form
    {
        Button[] Array_of_buttons;
        Logic_of_a_main_test_page Test_page_logic;
        public Test()
        {
            InitializeComponent();
            this.Array_of_buttons = new Button[6] { Answer1, Answer2, Answer3, Answer4, Answer5, Answer6 };
            this.Test_page_logic = Program.Main_test_page;
        }

        private void Test_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            this.Show_answers_on_page();
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Answer1_Click(object sender, EventArgs e)
        {
            this.answer_button_logic(0);
        }

        private void Answer2_Click(object sender, EventArgs e)
        {
            this.answer_button_logic(1);
        }

        private void Answer3_Click(object sender, EventArgs e)
        {
            this.answer_button_logic(2);
        }

        private void Answer4_Click(object sender, EventArgs e)
        {
            this.answer_button_logic(3);
        }

        private void Answer5_Click(object sender, EventArgs e)
        {
            this.answer_button_logic(4);
        }

        private void Answer6_Click(object sender, EventArgs e)
        {
            this.answer_button_logic(5);
        }

        private void Previous_question_button_Click(object sender, EventArgs e)
        {
            this.Test_page_logic.Go_to_the_previous_question();
            this.Bottom_buttons_logic();
            this.Show_answers_on_page();
        }

        private void Next_question_Click(object sender, EventArgs e)
        {
            if (this.Test_page_logic.Is_it_last_question())
            {
                this.Test_page_logic.Messege_at_the_end();
                Program.w_Test_Choice.Show();
                Program.w_Test.Hide();
                return;
            }
            this.Test_page_logic.Go_to_the_next_question();
            this.Bottom_buttons_logic();
            this.Show_answers_on_page();
        }

        private void answer_button_logic(int button_id)
        {
            this.Disable_buttons(button_id);
            this.Next_question.Enabled = true;
            this.Test_page_logic.Set_answer_button(button_id);
        }
        private void Disable_buttons(int button_id)
        {
            this.Array_of_buttons[button_id].Enabled = false;
            for (int i = 0; i < this.Array_of_buttons.Length; i++)
            {
                if (i != button_id)
                {
                    this.Array_of_buttons[i].Enabled = true;
                }
            }
        }
        private void Show_answers_on_page()
        {
            this.Count_of_questions.Text = this.Test_page_logic.Text_for_counter();
            this.Question_title.Text = this.Test_page_logic.Get_question_text();
            this.Answer_texts(this.Test_page_logic.Get_array_of_answers());
            int but_id = this.Test_page_logic.Selected_answer_to_a_current_question();
            if (but_id != -1)
            {
                this.Disable_buttons(but_id);
            }
            this.Bottom_buttons_logic();
        }
        private void Answer_texts(Answer[] array_of_answerss)
        {
            short size_of_answer_array = (short)array_of_answerss.Length;
            for (int i = 0; i < size_of_answer_array; i++)
            {
                this.Array_of_buttons[i].Text = array_of_answerss[i].text;
                this.Array_of_buttons[i].Visible = true;
                this.Array_of_buttons[i].Enabled = true;
            }
            if (size_of_answer_array < 6) this.Hide_unused_buttons(size_of_answer_array);
        }
        private void Hide_unused_buttons(int index_from_witch_needs_to_be_hide)
        {
            for (int i = index_from_witch_needs_to_be_hide; i < 6; i++)
            {
                this.Array_of_buttons[i].Visible = false;
                this.Array_of_buttons[i].Enabled = false;
            }
        }

        private void Bottom_buttons_logic()
        {
            if (this.Test_page_logic.Is_it_first_question())
            {
                this.Previous_question_button.Enabled = false;
                this.Next_question.Enabled = false;
                if (this.Test_page_logic.Selected_answer_to_a_current_question() != -1)
                    this.Next_question.Enabled = true;
                this.Next_question.Text = "Далее";
                return;
            }
            else if (this.Test_page_logic.Is_it_last_question())
            {
                this.Previous_question_button.Enabled = true;
                this.Finish_test_button();
                return;
            }
            if (this.Test_page_logic.Is_there_only_one_page())
            {
                this.Previous_question_button.Enabled = false;
                this.Finish_test_button();
                return;
            }
            this.Previous_question_button.Enabled = true;
            this.Next_question.Enabled = false;
            if(this.Test_page_logic.Selected_answer_to_a_current_question() != -1)
                this.Next_question.Enabled = true;
            this.Next_question.Text = "Далее";
        }

        private void Finish_test_button()
        {
            this.Next_question.Enabled = false;
            this.Next_question.Text = "Закончить";
            if (this.Test_page_logic.Selected_answer_to_a_current_question() != -1)
            {
                this.Next_question.Enabled = true;
            }

        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Program.w_Test.Hide();
            Program.w_Test_Choice.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
