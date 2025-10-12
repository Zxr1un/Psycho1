using Microsoft.Data.Sqlite;
using PsychoVS2.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PsychoVS2
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
        public Question(int id = -1, string text = "none", List<Answer> answers = null)
        {
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
        public Points_cods(int id = -1, string type = "none", int value = -1001)
        {
            this.id = id;
            this.type = type;
            this.value = value;
        }
        public int id;
        public string type;
        public int value;
    }



    /*Базовые установки
    1) Значения в классах по умолчанию означают, что они будут игнорироваться (далее пример)
    2) При загрузке всех тестов, загружается всё кроме набора вопросов к ним
    3) Комментарии пишем на русском
    */


    public class DB_work
    {
        public List<Psycho_Test> tests = new List<Psycho_Test>();
        public Psycho_Test current_test = null;

        private string dbPath;
        private string connectionString;

        private void init_db_path()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            dbPath = Path.Combine(baseDir, @"..\..\tests.db");

            // Microsoft.Data.Sqlite не использует параметр Version
            connectionString = $"Data Source={dbPath};";
            MessageBox.Show(connectionString);
        }

        public void load_all_tests()
        {
            tests.Clear();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string table = "SELECT * FROM tests";

                using (var cmd = new SqliteCommand(table, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int testId = reader.GetInt32(reader.GetOrdinal("id"));
                        string title = reader["title"].ToString();
                        string type = reader["type"].ToString();
                        string author = reader["author"].ToString();

                        var test = new Psycho_Test(testId, title, type, author);
                        tests.Add(test);

                        using (var countCmd = new SqliteCommand("SELECT COUNT(*) FROM questions WHERE test_id = @id", conn))
                        {
                            countCmd.Parameters.AddWithValue("@id", testId);
                            test.amm_of_questions = Convert.ToInt32(countCmd.ExecuteScalar());
                        }
                    }
                }
            }
        }

        public Psycho_Test load_current_test(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SqliteCommand("SELECT * FROM tests WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int testId = reader.GetInt32(reader.GetOrdinal("id"));
                            string title = reader["title"].ToString();
                            string type = reader["type"].ToString();
                            string author = reader["author"].ToString();

                            current_test = new Psycho_Test(testId, title, type, author);

                            using (var countCmd = new SqliteCommand("SELECT COUNT(*) FROM questions WHERE test_id = @id", conn))
                            {
                                countCmd.Parameters.AddWithValue("@id", testId);
                                current_test.amm_of_questions = Convert.ToInt32(countCmd.ExecuteScalar());
                            }

                            // Загружаем вопросы
                            using (var qCmd = new SqliteCommand("SELECT * FROM questions WHERE test_id = @testId", conn))
                            {
                                qCmd.Parameters.AddWithValue("@testId", testId);
                                using (var qReader = qCmd.ExecuteReader())
                                {
                                    current_test.questions = new List<Question>();

                                    while (qReader.Read())
                                    {
                                        int questionId = qReader.GetInt32(qReader.GetOrdinal("id"));
                                        string questionText = qReader["text"].ToString();

                                        var question = new Question(questionId, questionText, new List<Answer>());

                                        // Загружаем ответы
                                        using (var aCmd = new SqliteCommand("SELECT * FROM answers WHERE question_id = @qid", conn))
                                        {
                                            aCmd.Parameters.AddWithValue("@qid", questionId);
                                            using (var aReader = aCmd.ExecuteReader())
                                            {
                                                while (aReader.Read())
                                                {
                                                    int answerId = aReader.GetInt32(aReader.GetOrdinal("id"));
                                                    string answerText = aReader["text"].ToString();

                                                    var answer = new Answer(answerId, answerText, new List<Points_cods>());

                                                    // Загружаем баллы
                                                    using (var pCmd = new SqliteCommand("SELECT * FROM points WHERE answer_id = @aid", conn))
                                                    {
                                                        pCmd.Parameters.AddWithValue("@aid", answerId);
                                                        using (var pReader = pCmd.ExecuteReader())
                                                        {
                                                            while (pReader.Read())
                                                            {
                                                                int pointId = pReader.GetInt32(pReader.GetOrdinal("id"));
                                                                string pointType = pReader["point_type"].ToString();
                                                                int pointValue = pReader["value"] != DBNull.Value ? Convert.ToInt32(pReader["value"]) : 0;

                                                                var point = new Points_cods(pointId, pointType, pointValue);
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

                            return current_test;
                        }
                    }
                }
            }

            current_test = null;
            return null;
        }

        public void show_all_tests()
        {
            string output = string.Join("\n", tests.Select(t =>
                $"{t.id} | {t.name} | {t.type} | {t.author} | вопросов: {t.amm_of_questions}"));

            MessageBox.Show(output);
        }

        public void show_current_test()
        {
            if (current_test == null)
            {
                MessageBox.Show("NULL");
                return;
            }

            string output = $"{current_test.id} | {current_test.name} | {current_test.type} | {current_test.author} | вопросов: {current_test.amm_of_questions}";
            MessageBox.Show(output);
        }

        public DB_work()
        {
            init_db_path();
            load_all_tests();
        }
    }

    internal class Work_with_DB
    {
    }
}
