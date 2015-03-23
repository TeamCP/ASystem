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

namespace ASystem
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btb_Enter_Click(object sender, RoutedEventArgs e)
        {
            int AuthResult = Auth.TryAuth(Tb_Login_LW_.Text, Tb_Pass_LW_.Text);
            
            MainWindow mainWindow = new MainWindow();

            switch (AuthResult)
            {
                case -1: 
                    NoConnectionWindow offlineRegister = new NoConnectionWindow();
                    this.Hide();
                    offlineRegister.Show();
                    break;
                    
                case -2:
                    MessageBox.Show("Пользователя с таким логином и паролем не существует. Проверьте вводимые данные.");
                    break;

                case 0:
                    this.Hide();
                    mainWindow.Show();
                    break;
                
                case 1:
                    this.Hide();
                    mainWindow.Show();
                    break;            }
            
        }

        private void btb_Сancel_Click(object sender, RoutedEventArgs e)
        {
            Application myApp = Application.Current;
            myApp.Shutdown();
        }
    }
}
