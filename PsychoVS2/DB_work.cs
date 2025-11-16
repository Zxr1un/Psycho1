using Microsoft.Data.Sqlite;
using NCalc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;


namespace PsychoVS2
{
    public class Results
    {
        public string condition = "";
        public string result = "";
        public int order = 0;
    }

    /*Классы для хранения тестов*/

    //Test storage
    public class Psycho_Test
    {
        public Psycho_Test(int id, string title, string type = "none", string author = "none", List<Question> questions = null, byte[] imageData = null, string description = "No discription")
        {
            this.id = id;
            this.name = title;
            this.type = type;
            this.author = author;
            this.questions = questions;
            this.description = description;

            // Загружаем картинку из БД
            if (imageData != null)
            {
                this.image = LoadBitmapImage(imageData);
            }
            else
            {
                // Ищем корневую папку Psycho1
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                DirectoryInfo root = null;
                while (dir != null)
                {
                    if (dir.Name.Equals("Psycho1", StringComparison.OrdinalIgnoreCase))
                    {
                        root = dir;
                        break;
                    }
                    dir = dir.Parent;
                }

                if (root == null)
                    throw new FileNotFoundException("Не удалось найти папку Psycho1!");

                // Формируем путь к картинке
                string path = Path.Combine(root.FullName, "PsychoVS2", "Image", "testImage2.png");

                if (!File.Exists(path))
                    throw new FileNotFoundException($"Файл не найден: {path}");

                this.image = LoadBitmapImage(File.ReadAllBytes(path));
            }

            this.description = description;
        }

        public int id = -1;
        public string name;
        public string description;
        public string type;
        public string author;
        public int amm_of_questions = 0;

        public List<Question> questions;
        public BitmapImage image;

        //Функция конвертации
        private BitmapImage LoadBitmapImage(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad; // Загружает полностью в память
                bmp.StreamSource = stream;
                bmp.EndInit();
                bmp.Freeze(); // чтобы можно было спокойно использовать в UI из любого потока
                return bmp;
            }
        }
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
        public int order;
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

    //****************WORK WITH DB*******************************************
    public class DB_work
    {


        public List<Psycho_Test> tests = new List<Psycho_Test>(0);
        public Psycho_Test current_test = null;

        public string version = "3";
        //"Data Source=Psycho1\\PsychoAT\\tests.db;Version=3;"
        private string dbPath = "Data Source=Psycho1\\PsychoAT\\tests.db;Version=3;";
        //that won't work, but latter it wil be correctly initialized
        private string connectionString = "";
        //command to connect
        //automatic search of PATH

        //private void init_db_path()
        //{
        //    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        //    dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tests.db");
        //    connectionString = $"Data Source={dbPath};Version={version};";
        //    MessageBox.Show(connectionString);
        //}
        private void init_db_path()
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;

            // Ищем родительскую папку "Psycho1"
            DirectoryInfo dir = new DirectoryInfo(exeDir);
            DirectoryInfo root = null;

            while (dir != null)
            {
                if (dir.Name.Equals("Psycho1", StringComparison.OrdinalIgnoreCase))
                {
                    root = dir;
                    break;
                }
                dir = dir.Parent;
            }

            if (root == null)
            {
                MessageBox.Show("Не удалось найти папку Psycho1!");
                return;
            }

            // Путь к tests.db внутри "Делатель тестов -3006"
            string dbFolder = Path.Combine(root.FullName, "Делатель тестов -3006");
            dbPath = Path.Combine(dbFolder, "tests.db");

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

                        // Загружаем картинку из таблицы images
                        byte[] image = null;
                        using (SQLiteCommand imgCmd = new SQLiteCommand(
                            "SELECT image_data FROM images WHERE test_id = @tid LIMIT 1", conn))
                        {
                            imgCmd.Parameters.AddWithValue("@tid", testId);

                            object result = imgCmd.ExecuteScalar();
                            if (result != DBNull.Value && result != null)
                                image = (byte[])result;
                        }


                        tests.Add(new Psycho_Test(testId, title, type, author, null, image));

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


                            // Загружаем картинку из таблицы images
                            byte[] image = null;
                            using (SQLiteCommand imgCmd = new SQLiteCommand(
                                "SELECT image_data FROM images WHERE test_id = @tid LIMIT 1", conn))
                            {
                                imgCmd.Parameters.AddWithValue("@tid", testId);

                                object result = imgCmd.ExecuteScalar();
                                if (result != DBNull.Value && result != null)
                                    image = (byte[])result;
                            }


                            current_test = new Psycho_Test(testId, title, type, author, null, image);

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

        public Results[] get_results(int test_id)
        {
            List<Results> resultsList = new List<Results>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT condition, result_text FROM results WHERE test_id = @tid";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tid", test_id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string condition = reader["condition"] != DBNull.Value ? reader["condition"].ToString() : "";
                            string resultText = reader["result_text"] != DBNull.Value ? reader["result_text"].ToString() : "";
                            int order1 = Convert.ToInt32(reader["position"]);

                            Results res = new Results
                            {
                                condition = condition,
                                result = resultText,
                                order = order1
                            };
                            //MessageBox.Show(res.condition);
                            resultsList.Add(res);
                        }
                    }
                }
            }
            resultsList.Sort((a, b) => a.order.CompareTo(b.order));
            return resultsList.ToArray();
        }

        public DB_work()
        {
            init_db_path();
            this.load_all_tests();
            show_all_tests();
            load_current_test(2);
            show_current_test();
        }
    }
}
