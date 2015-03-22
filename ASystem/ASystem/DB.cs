using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
namespace ASystem
{
    class DB
    {
        protected const string DBname = "db";
        protected const string Host = "localhost";
        protected const string DBusername = "root";
        protected const string DBpassword = "";
        protected static int exception_number;

        public static void AddNewUser(string login, string pass, string phone_number, int access_level, string whois)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", Host, DBusername, DBpassword, DBname);
            
            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;

            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));                
                return;
            }

            try
            {
                Query.CommandText = String.Format("INSERT INTO users(login, pass, phone_number, access_level, whois) values('{0}', '{1}', '{2}', '{3}', '{4}')", login, pass, phone_number, access_level, whois);
                Query.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при добавлении новой учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));    
                
                return;

            }

            MessageBox.Show(String.Format("Пользователь {0} успешно зарегистрирован.",login));

            MySQLConnection.Close();
        }

        public static void AddNewEvent(int user_id, string description, string place, DateTime DT, int importance_level)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", Host, DBusername, DBpassword, DBname);

            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;

            exception_number = 0;

            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));
                exception_number++;
                return;
            }

            string datestring = DT.ToString("yyyy-MM-dd");
            string timestring = DT.ToString("HH:mm:ss");

            try
            {
                Query.CommandText = String.Format("INSERT INTO events(user_id, description, place, date, time, importance_level)" +
                                                   " values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", user_id, description, place, datestring, timestring, importance_level);
                Query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при добавлении новой учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));
                exception_number++;
                return;
            }

            MySQLConnection.Close();
            
        }

        public static int Exception_number
        {
            get { return exception_number; }
        }
    }

    class EditUserInfo: DB
    {
        public static void Password(int id, string new_pass)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", Host, DBusername, DBpassword, DBname);

            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;

            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     
                
                return;
            }

            try
            {
                Query.CommandText = String.Format("UPDATE users SET pass = '{0}' WHERE id = '{1}'",new_pass,id);
                Query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при редактировании учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     

                return;
            }

            MySQLConnection.Close();
        }

        public static void Phone_number(int id , string new_pn)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", Host, DBusername, DBpassword, DBname);

            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;

            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     
                return;
            }

            try
            {
                Query.CommandText = String.Format("UPDATE users SET phone_number = '{0}' WHERE id = '{1}'", new_pn, id);
                Query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при редактировании учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     
                return;
            }

            MySQLConnection.Close();
        }

        public static void Access_level(int id, int new_al)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", Host, DBusername, DBpassword, DBname);

            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;

            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     

                return;
            }

            try
            {
                Query.CommandText = String.Format("UPDATE users SET access_level = '{0}' WHERE id = '{1}'", new_al, id);
                Query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при редактировании учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     
                return;
            }

            MySQLConnection.Close();
        }

        public static void Who_is(int id, string new_info)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", Host, DBusername, DBpassword, DBname);

            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;

            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     
                return;
            }

            try
            {
                Query.CommandText = String.Format("UPDATE users SET whois = '{0}' WHERE id = '{1}'", new_info, id);
                Query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при редактировании учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));     
                return;
            }

            MySQLConnection.Close();
        }
    }
}
