using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Drawing;
using WinForms = System.Windows.Forms;
using System.Windows.Media;
using AC.AvalonControlsLibrary.Controls;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ASystem
{
    public partial class MainWindow : Window 
    {
        private ContextMenu _trayMenu { get; set; }
        private WinForms.NotifyIcon _trayIcon { get; set; }
        private DateTime _date;

        private static MyCalendar myCalendar = new MyCalendar();
        static Button[,] arrButton;
        static bool _buttonChecker;
        static int _indexButton;
        public MainWindow()
        {
            InitializeComponent();

            _date = DateTime.Now;

            #region Initialization icon in the tray
            _trayIcon = new WinForms.NotifyIcon();
            _trayIcon.Icon = new Icon(System.AppDomain.CurrentDomain.BaseDirectory + @"\resource\icon\myIcon.ico");
            _trayIcon.Visible = true;
            _trayIcon.Text = "Awareness System";
            _trayIcon.ContextMenu = new WinForms.ContextMenu();
            _trayIcon.MouseDoubleClick += Icon_Click;
            _trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(trayIcon_OnMouseClick);

            _trayMenu = (ContextMenu)FindResource("_trayMenu");
            #endregion

            arrButton = new Button[,]   {{b11, b12, b13, b14, b15, b16, b17},
                                         {b21, b22, b23, b24, b25, b26, b27},
                                         {b31, b32, b33, b34, b35, b36, b37},
                                         {b41, b42, b43, b44, b45, b46, b47},
                                         {b51, b52, b53, b54, b55, b56, b57},
                                         {b61, b62, b63, b64, b65, b66, b67}};

            FillingCalendarNumbers();
        }

        public static Button SearchButton(string content)
        {
            for (int i = 0; i < arrButton.GetLength(0); i++)
            {
                for (int q = 0; q < arrButton.GetLength(1); q++)
                {
                    if ((string)arrButton[i, q].Content == content)
                        return arrButton[i, q];
                }
            }
            return null;
        }

        #region Procedure opening/closing program

        void trayIcon_OnMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _trayMenu.IsOpen = true;
            }
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            _trayIcon.Visible = false;
            Application myApp = Application.Current;
            myApp.Shutdown();
        }
        #endregion

        public void FillingCalendarNumbers()
        {
            Lbl_MonthAndYear.Content = myCalendar.GetMonthAndYear();

            for (int i = 0; i < 6; i++)
            {
                for (int q = 0; q < 7; q++)
                {
                    arrButton[i, q].Content = "";
                    arrButton[i, q].Style = (Style)arrButton[i, q].FindResource("EmptyDay_btn");
                }
            }

            int startDayMonth = myCalendar.GetStartDayMonth();
            int amountDayMonth = myCalendar.GetAmountDayMonth();
            int numberDay = 0;
            bool cheak = false;

            for (int i = 0; i < 6; i++)
            {
                for (int q = startDayMonth; q < 7; q++)
                {
                    startDayMonth = 0;

                    numberDay++;
                    arrButton[i, q].Content = Convert.ToString(numberDay);
                    arrButton[i, q].Style = (Style)arrButton[i, q].FindResource("Day_btn");
                    if (numberDay == amountDayMonth) 
                    {
                        cheak = true;
                        break;               
                    }
                }

                if (cheak) { break; }
            }

            _date = myCalendar.GetAllDate();
            XmlDoc.MarksActiveDays(_date);
        }

        public void Btn_Day_OnClick(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedIndex = 1;
            _date = myCalendar.GetChangeDate(Convert.ToInt32(((Button)sender).Content));

            Lbl_Title.Content = myCalendar.GetMonthAndDay();

            MyStack.Children.Clear();

            XmlDoc xmlDoc = new XmlDoc();

            int index = 1;
            do
            {
                string content = xmlDoc.ReadXmlFile(_date, 
                    index);

                Label newLabel = new Label();
                newLabel.Content = content;
                MyStack.Children.Add(newLabel);
                newLabel.Style= (Style)newLabel.FindResource("Doing_lbl");
                index++;
            }
            while (index <= XmlDoc.GetAmountEvents());      
        }

        public void Doing_lbl_OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Button newButton = new Button();
            var style = FindResource("Raport_btn") as Style;
            newButton.Style = style;
            if(_buttonChecker)
            {
                MyStack.Children.RemoveAt(_indexButton);
            }
            int index = MyStack.Children.IndexOf((UIElement)sender);
            _indexButton = index + 1;

            MyStack.Children.Insert(_indexButton, newButton);
            _buttonChecker = true;
        }

        public static void GetButtonDayActive(Button btn, int priority)
        {
            switch(priority)
            {
                case 1:
                    btn.Style = (Style)btn.FindResource("ActiveDay_pr1_btn");
                    break;
                case 2:
                    btn.Style = (Style)btn.FindResource("ActiveDay_pr2_btn");
                    break;
                case 3:
                    btn.Style = (Style)btn.FindResource("ActiveDay_pr3_btn");
                    break;
                case 0:
                   // btn.Style = (Style)btn.FindResource("Day_btn");
                    break;
            }
            
        }

        private void Btn_Raport_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_ToMainTab_Click(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedIndex = 0;
            _indexButton = 0;
            _buttonChecker = false;
        }

        private void Btn_ArrowLeft_Click(object sender, RoutedEventArgs e)
        {
            myCalendar.PreviousMonth();            
            FillingCalendarNumbers();
        }

        private void Btn_ArrowRight_Click(object sender, RoutedEventArgs e)
        {
            myCalendar.NextMonth();
            FillingCalendarNumbers();
        }

        private void Lbl_MonthAndYear_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            myCalendar.Today();
            FillingCalendarNumbers();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Btn_Settings_Click(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedIndex = 2;
        }

        private void Btn_CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedIndex = 3;
        }      

        private void Btn_AddEvent_Click(object sender, RoutedEventArgs e)
        {
            int importance = 0;
            if (Cb_MinPriority.IsChecked == true) importance = 1;
            if (Cb_MidPriority.IsChecked == true) importance = 2;
            if (Cb_MaxPriority.IsChecked == true) importance = 3;

            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>TIME PICKER THERE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            // Сюда нужно читать данные из таймпикера в поля для времени.
            int hour = 12;
            int minute = 0;

            DateTime DT = new DateTime(_date.Year, _date.Month, _date.Day, hour, minute, 0);

            // Тут будет метод возвращающий массив с ID выбранных в листбоксе юзеров. 
            int[] id_array = { 1, 2 };

            foreach (int id in id_array)
            {
                if (DB.Exception_number == 0) DB.AddNewEvent(id, Tb_TextEvent.Text, Tb_PlaceEvent.Text, DT, importance);                
            }

            if (DB.Exception_number == 0) MessageBox.Show(String.Format("Событиt успешно добавлено."));           

        }
    }
}
