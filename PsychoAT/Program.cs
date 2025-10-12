using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace PsychoAT
{

    /*Классы для хранения тестов*/

    //Test storage
    public class Psycho_Test
    {

        //для конкретного теста
        public Psycho_Test(int id, string title, string type = "none", string author = "none", List<Question> questions = null)
        {
            this.id = id;
            this.name = title;
            this.type = type;
            this.author = author;
            this.questions = questions; //список вопросов
        }

        public int id = -1; //id в БД
        public string name; // Название
        public string type; // Тип
        public string author; // Автор
        public int amm_of_questions = 0; // Количество вопросов в тесте

        public List<Question> questions; //список вопросов

    }
    //Question storage
    public class Question
    {
        public Question(int id = -1, string text = "none", List<Answer> answers = null) {
            this.id = id;
            this.text = text;
            this.answers = answers;
        }
        public int id;
        public string text;
        public List<Answer> answers;

    }
    //Answer storage
    public class Answer
    {
        public Answer(int id = -1, string text = "none", List<Points_cods> points_Cods = null)
        {
            this.id = id;
            this.text = text;
            this.points_cods = points_Cods;
        }
        public int id;
        public string text;
        public List<Points_cods> points_cods;
    }
    //Storage of type and value of points of current answer
    public class Points_cods
    {
        public Points_cods(int id = -1, string type = "none", int value = - 1001) {
            this.id =id;
            this.type =type;
            this.value =value;
        }
        public int id;
        public string type;
        public int value;
    }



    /*Базовые установки
    1) Значения в классах по умолчанию означают, что они будут игнорироваться (далее пример)
    2) При загрузке всех тестов, загружается всё кроме набора вопросов к ним
    3) Комментарии пишем на русском


    /*Классы и методы загрузки тестов из БД*/
    public class DB_work
    {

        public List<Psycho_Test> tests = new List<Psycho_Test>(0);
        public Psycho_Test current_test = null;

        public string version = "3";
        private string dbPath = "Data Source=Psycho1\\PsychoAT\\tests.db;Version=3;";
        //that won't work, but latter it wil be correctly initialized
        private string connectionString = "";
        //command to connect
        //automatic search of PATH
        private void init_db_path()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            dbPath = Path.Combine(baseDir, @"..\..\tests.db");
            connectionString = $"Data Source={dbPath};Version={version};";
            MessageBox.Show(connectionString);
        }

        public void load_all_tests()
        {

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string table = "SELECT * FROM tests";   // Команда на таблицу тестов
                using (SQLiteCommand cmd = new SQLiteCommand(table, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        int testId = Convert.ToInt32(reader["id"]);
                        string title = reader["title"].ToString();
                        string type = reader["type"].ToString();
                        string author = reader["author"].ToString();

                        tests.Add(new Psycho_Test(testId, title, type, author));

                        using (SQLiteCommand countCmd = new SQLiteCommand("SELECT COUNT(*) FROM questions WHERE test_id = @id", conn))
                        {
                            countCmd.Parameters.AddWithValue("@id", testId);
                            tests[tests.Count - 1].amm_of_questions = Convert.ToInt32(countCmd.ExecuteScalar());
                        }
                    }
                }
            }
        }
        public Psycho_Test load_current_test(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM tests WHERE id = @id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int testId = Convert.ToInt32(reader["id"]);
                            string title = reader["title"].ToString();
                            string type = reader["type"].ToString();
                            string author = reader["author"].ToString();
                            current_test = new Psycho_Test(testId, title, type, author);

                            using (SQLiteCommand countCmd = new SQLiteCommand("SELECT COUNT(*) FROM questions WHERE test_id = @id", conn))
                            {
                                countCmd.Parameters.AddWithValue("@id", testId);
                                current_test.amm_of_questions = Convert.ToInt32(countCmd.ExecuteScalar());
                            }

                            using (SQLiteCommand qCmd = new SQLiteCommand("SELECT * FROM questions WHERE test_id = @testId", conn))
                            {
                                qCmd.Parameters.AddWithValue("@testId", testId);

                                using (SQLiteDataReader qReader = qCmd.ExecuteReader())
                                {
                                    current_test.questions = new List<Question>();

                                    while (qReader.Read())
                                    {
                                        int questionId = Convert.ToInt32(qReader["id"]);
                                        string questionText = qReader["text"].ToString();

                                        Question question = new Question(questionId, questionText, new List<Answer>());

                                        // Загружаем ответы для этого вопроса
                                        using (SQLiteCommand aCmd = new SQLiteCommand("SELECT * FROM answers WHERE question_id = @qid", conn))
                                        {
                                            aCmd.Parameters.AddWithValue("@qid", questionId);

                                            using (SQLiteDataReader aReader = aCmd.ExecuteReader())
                                            {
                                                while (aReader.Read())
                                                {
                                                    int answerId = Convert.ToInt32(aReader["id"]);
                                                    string answerText = aReader["text"].ToString();

                                                    Answer answer = new Answer(answerId, answerText, new List<Points_cods>());

                                                    // Загружаем баллы для этого ответа
                                                    using (SQLiteCommand pCmd = new SQLiteCommand("SELECT * FROM points WHERE answer_id = @aid", conn))
                                                    {
                                                        pCmd.Parameters.AddWithValue("@aid", answerId);

                                                        using (SQLiteDataReader pReader = pCmd.ExecuteReader())
                                                        {
                                                            while (pReader.Read())
                                                            {
                                                                int pointId = Convert.ToInt32(pReader["id"]);
                                                                string pointType = pReader["point_type"].ToString();
                                                                int pointValue = pReader["value"] != DBNull.Value ? Convert.ToInt32(pReader["value"]) : 0;

                                                                Points_cods point = new Points_cods(pointId, pointType, pointValue);
                                                                answer.points_cods.Add(point);
                                                            }
                                                        }
                                                    }

                                                    question.answers.Add(answer);
                                                }
                                            }
                                        }

                                        current_test.questions.Add(question);
                                    }
                                }
                            }


                            return this.current_test;
                        }
                    }
                }
            }
            current_test = null; // если нет такого id
            return null;
        }

        public void show_all_tests()
        {
            string output = "";
            foreach (Psycho_Test a in tests)
            {
                output += a.id.ToString() + " | " + a.name + " | " + a.type + " | " + a.author + "| вопросов: " + a.amm_of_questions + "\n";
            }
            MessageBox.Show(output);
        }
        public void show_current_test()
        {
            if (current_test == null)
            {
                MessageBox.Show("NULL");
                return;
            }
            string output = current_test.id.ToString() + " | " + current_test.name + " | " + current_test.type + " | " + current_test.author + "| вопросов: " + current_test.amm_of_questions + "\n";
            MessageBox.Show(output);
        }

        public DB_work()
        {
            init_db_path();
            this.load_all_tests();
        }
    }





    internal static class Program
    {
        public static DB_work db = new DB_work();
        public static Work_with_test_choice_page Test_choise_logic = new Work_with_test_choice_page(db);
        public static Logic_of_a_main_test_page Main_test_page = new Logic_of_a_main_test_page(db);

        public static int window = 0;

        public static Start w_Start;
        public static Test_choice w_Test_Choice;
        public static Test_Start w_Test_Start;
        public static Test w_Test;
        public static Result w_Result;
        public static Statistics w_Stat;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            db.load_current_test(2);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            w_Start = new Start();
            w_Test_Choice = new Test_choice();
            w_Test = new Test();
            w_Test_Start = new Test_Start();
            w_Result = new Result();
            w_Stat = new Statistics();


            Application.Run(w_Start);


        }
    }

    class Work_with_test_choice_page
    {
        public Work_with_test_choice_page(DB_work DB_data) {
            this.Init_array_of_pages(DB_data);
        }
        private int Number_of_a_page_we_currently_on = 0;
        private Psycho_Test[][] Array_of_tests_divided_by_pages;
        private bool Is_error_occured = false;

        private void Init_array_of_pages(DB_work DB_data)
        {
            int size_of_list = DB_data.tests.Count;
            if (size_of_list == 0)
            {
                this.Is_error_occured = true;
                return;
            }
            int number_of_pages = size_of_list / 5 + 1;
            this.Array_of_tests_divided_by_pages = new Psycho_Test[number_of_pages][];
            for (int k = 0; k < number_of_pages; k++)
            {
                this.Array_of_tests_divided_by_pages[k] = new Psycho_Test[5];
            }
            int i = 0; short j = 0;
            foreach (Psycho_Test test in DB_data.tests)
            {
                if (j < 5)
                {
                    this.Array_of_tests_divided_by_pages[i][j] = test;
                    j++;
                }
                else { i++; j = 0; }
            }
        }

        public bool Check_for_an_error()
        {
            return this.Is_error_occured;
        }
        public Psycho_Test[] Test_for_show_before_load()
        {
            return this.Internal_for_load_before_shown();
        }
        public Psycho_Test[] Get_on_previous_page()
        {
            return this.Internal_shift_down();
        }
        public Psycho_Test[] Get_on_next_page()
        {
            return this.Internal_shift_up();
        }
        private Psycho_Test[] Internal_for_load_before_shown()
        {
            return this.Array_of_tests_divided_by_pages[Number_of_a_page_we_currently_on];
        }
        private Psycho_Test[] Internal_shift_down()
        {
            this.Number_of_a_page_we_currently_on--;
            return this.Array_of_tests_divided_by_pages[this.Number_of_a_page_we_currently_on];
        }
        private Psycho_Test[] Internal_shift_up()
        {
            this.Number_of_a_page_we_currently_on++;
            return this.Array_of_tests_divided_by_pages[this.Number_of_a_page_we_currently_on];
        }
        public bool Is_page_last()
        {
            return this.Number_of_a_page_we_currently_on == this.Array_of_tests_divided_by_pages.Length-1;
        }
        public bool Is_page_first()
        {
            return this.Number_of_a_page_we_currently_on == 0;
        }
        public bool Is_it_only_page()
        {
            return this.Array_of_tests_divided_by_pages.Length == 1;
        }
    }

    class Logic_of_a_main_test_page
    {
        public Logic_of_a_main_test_page(DB_work DB_data) { this.db = DB_data; }

        private Psycho_Test Current_test;
        private DB_work db;
        private string[] Array_of_questions_texts;
        private Answer[][] Array_of_answers_to_each_question;
        private int Current_question_on_a_page = 0;
        private int Number_of_questions;
        private int[] Selected_answers_array;
        Dictionary<string, int> point_collector = new Dictionary<string, int>();

        public void Set_current_test(Psycho_Test Chosen_test)
        {
            this.Internal_set_current_test(Chosen_test);
        }
        private void Internal_set_current_test(Psycho_Test Chos_test)
        {
            this.Current_test = this.db.load_current_test(Chos_test.id);
            this.Initialize_both_internal_array();
           
        }
        private void Initialize_both_internal_array()
        {
            int size_of_questions_list = this.Current_test.questions.Count;
            if(size_of_questions_list == 0)
            {
                MessageBox.Show("No questions in test!! Check BD!!!!", "BD error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Number_of_questions = size_of_questions_list;
            this.Array_of_questions_texts = new string[size_of_questions_list];
            this.Selected_answers_array = new int[size_of_questions_list];
            for (int i = 0; i < size_of_questions_list; i++)
            {
                this.Selected_answers_array[i] = -1;
            }
            short j = 0;
            foreach (Question raw_question in this.Current_test.questions)
            {
                this.Array_of_questions_texts[j] = raw_question.text;
                j++;
            }
            this.Array_of_answers_to_each_question = new Answer[size_of_questions_list][];
            j = 0;
            foreach(Question a in this.Current_test.questions)
            {
                this.Array_of_answers_to_each_question[j] = a.answers.ToArray();
                j++;
            }
        }
        public Answer[] Get_array_of_answers()
        {
            return this.Internal_get_arra_of_questions();
        }
        private Answer[] Internal_get_arra_of_questions()
        {
            return this.Array_of_answers_to_each_question[Current_question_on_a_page];
        }
        public string Get_question_text()
        {
            return this.Internal_get_quest_name();
        }
        private string Internal_get_quest_name()
        {
            return this.Array_of_questions_texts[this.Current_question_on_a_page];
        }
        public bool Is_it_last_question()
        {
            return this.Current_question_on_a_page == this.Array_of_questions_texts.Length-1;
        }
        public bool Is_it_first_question()
        {
            return this.Current_question_on_a_page == 0;
        }
        public bool Is_there_only_one_page()
        {
            return this.Array_of_questions_texts.Length == 1;
        }
        public void Go_to_the_next_question()
        {
            this.Current_question_on_a_page++;
        }
        public void Go_to_the_previous_question()
        {
            this.Current_question_on_a_page--;
        }
        public string Text_for_counter()
        {
            return (this.Current_question_on_a_page+1).ToString() + "/" + this.Number_of_questions.ToString();
        }
        public int Selected_answer_to_a_current_question()
        {
            return this.Selected_answers_array[this.Current_question_on_a_page];
        }
        public void Set_answer_button(int button_id)
        {
            this.Selected_answers_array[this.Current_question_on_a_page] = button_id;
        }
        public void Messege_at_the_end()
        {
            for (int i = 0; i < this.Number_of_questions; i++)
            {
                Points_cods[] temp = this.Array_of_answers_to_each_question[i][this.Selected_answers_array[i]].points_cods.ToArray();
                foreach (Points_cods point_code in temp)
                {
                    if (this.point_collector.ContainsKey(point_code.type))
                    {
                        this.point_collector[point_code.type] += point_code.value;
                        continue;
                    }
                    this.point_collector.Add(point_code.type, point_code.value);
                }
            }
            string text_for_messege = "";
            foreach (KeyValuePair<string, int> kvp in this.point_collector)
            {
                text_for_messege += $"type - {kvp.Key}, value - {kvp.Value}\n";
            }
            MessageBox.Show(text_for_messege, "resaults", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
