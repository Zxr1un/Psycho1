using PsychoVS2.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PsychoVS2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
 
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //db.load_current_test(2);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //w_Start = new Start();
            //w_Test_Choice = new Test_choice();
            //w_Test = new Test();
            //w_Test_Start = new Test_Start();
            //w_Result = new Result();
            //w_Stat = new Statistics();


            //Application.Run(w_Start);

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
