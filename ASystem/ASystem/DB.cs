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
        protected const string DB_name = "db";
        protected const string DB_server = "localhost";
        protected const string DB_username = "root";
        protected const string DB_password = "";

        protected static int exception_number;
        protected static List<Event> events;
        protected static List<User> users;

        protected static User CurrentUser { get; set; }

        public static void AddNewUser(string login, string pass, string phone_number, int access_level, string whois)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при добавлении новой учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));

                return;

            }

            MessageBox.Show(String.Format("Пользователь {0} успешно зарегистрирован.", login));

            MySQLConnection.Close();
        }

        public static void AddNewEvent(int user_id, string description, string place, DateTime DT, int importance_level)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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

        public static void GetEventList()
        {
            events = new List<Event>();

            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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

            Query.CommandText = "SELECT * FROM events;";
            MySqlDataReader MyReader = Query.ExecuteReader();

            while (MyReader.Read())
            {
                //Console.WriteLine("{0} - {1} - {2} - {3} - {4} - {5} - {6}", MyReader.GetValue(0), MyReader.GetValue(1), MyReader.GetValue(2), MyReader.GetValue(3), MyReader.GetValue(4).ToString(), MyReader.GetValue(5).ToString(), MyReader.GetValue(6));

                DateTime tmp_date = Convert.ToDateTime(MyReader.GetValue(4));
                TimeSpan tmp_time = TimeSpan.Parse(MyReader.GetValue(5).ToString());
                DateTime tmp_DT = new DateTime(tmp_date.Year, tmp_date.Month, tmp_date.Day, tmp_time.Hours, tmp_time.Minutes, tmp_time.Seconds);
                events.Add(new Event(Convert.ToInt32(MyReader.GetValue(0)), Convert.ToInt32(MyReader.GetValue(1)), MyReader.GetValue(2).ToString(), MyReader.GetValue(3).ToString(), tmp_DT, Convert.ToInt32(MyReader.GetValue(6))));
            }

            MySQLConnection.Close();
        }

        public static void GetUserList()
        {
            users = new List<User>();

            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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

            Query.CommandText = "SELECT id,login,whois,phone_number,access_level FROM users;";
            MySqlDataReader MyReader = Query.ExecuteReader();

            while (MyReader.Read())
            {
                users.Add(new User(Convert.ToInt32(MyReader.GetValue(0)), MyReader.GetValue(1).ToString(), MyReader.GetValue(2).ToString(), MyReader.GetValue(3).ToString(), Convert.ToInt32(MyReader.GetValue(4))));
            }

            MySQLConnection.Close();
        }

        public static List<Event> Events
        {
            get { return events; }
        }

        public static List<User> Users
        {
            get { return users; }
        }

        public static int Exception_number
        {
            get { return exception_number; }
        }
    }

    class User : DB
    {
        public int _id { get; set; }
        public string _login { get; set; }
        public string _whois { get; set; }
        public string _phone_number { get; set; }
        public int _access_level { get; set; }
        public User(int id, string login, string whois, string phone_number, int access_level)
        {
            _id = id;
            _login = login;
            _whois = whois;
            _phone_number = phone_number;
            _access_level = access_level;
        }

        public static void Edit_Password(int id, string new_pass)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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
                Query.CommandText = String.Format("UPDATE users SET pass = '{0}' WHERE id = '{1}'", new_pass, id);
                Query.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(String.Format("Возникла ошибка при редактировании учётной записи. \n {0} \n Error Number = {1}", ex.Message, ex.Number));

                return;
            }

            MySQLConnection.Close();
        }

        public static void Edit_Phone_number(int id, string new_pn)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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

        public static void Edit_Access_level(int id, int new_al)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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

        public static void Edit_Who_is(int id, string new_info)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

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

        public override string ToString()
        {
            return this._whois;
        }
    }

    class Event : DB
    {
        public int _id { get; set; }
        public int _user_id { get; set; }
        public string _description { get; set; }
        public string _place { get; set; }
        public DateTime _DT { get; set; }
        public int _importance_level { get; set; }

        public Event(int id, int user_id, string description, string place, DateTime DT, int importance_level)
        {
            _id = id;
            _user_id = user_id;
            _description = description;
            _place = place;
            _DT = DT;
            _importance_level = importance_level;
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", _description, _place, _DT.ToString(), _importance_level);
        }
    }

    class Auth : DB
    {
        public static int TryAuth(string login, string pass)
        {
            string ConnectionString = String.Format("server={0};Uid={1};Pwd={2};Database={3};", DB_server, DB_username, DB_password, DB_name);

            MySqlConnection MySQLConnection = new MySqlConnection(ConnectionString);
            MySqlCommand Query = new MySqlCommand();
            Query.Connection = MySQLConnection;
            try
            {
                MySQLConnection.Open();
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(String.Format("Невозможно соединиться с базой данных. Проверьте настройки соединения. \n {0} \n Error Number = {1}", ex.Message, ex.Number));
                return -1;
            }
            Query.CommandText = String.Format("SELECT id,whois,phone_number,access_level FROM users WHERE login='{0}' AND pass='{1}';", login, pass);

            MySqlDataReader MyReader = Query.ExecuteReader();

            List<User> tmp = new List<User>();

            while (MyReader.Read())
            {
                tmp.Add(new User(Convert.ToInt32(MyReader.GetValue(0)), login, MyReader.GetValue(1).ToString(), MyReader.GetValue(2).ToString(), Convert.ToInt32(MyReader.GetValue(3))));
            }

            if (tmp.Count == 1)
            {
                MySQLConnection.Close();
                CurrentUser = tmp[0];
                return CurrentUser._access_level;
            }
            else
            {
                MySQLConnection.Close();
                return -2;
            }
        }
    }
}
