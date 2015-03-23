using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ASystem
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
       public App()
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool cheak = XmlDoc.ReadConfig();

            if(cheak)
            {
                StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            }
            else
            {
                StartupUri = new Uri("LoginWindow.xaml", UriKind.Relative);
            }
            
        }
    }
}
