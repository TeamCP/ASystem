using System;

namespace ASystem
{
    class MyCalendar 
    {
        private DateTime _date;

        public MyCalendar()
        {         
            _date = DateTime.Now;;           
        }

        public DateTime GetAllDate()
        {
            return _date;
        }

        public string GetMonthAndYear()
        {
            string monthAndYear = _date.ToString("MMMM") + " " + _date.Year;
            return monthAndYear;
        }

        public string GetMonthAndDay()
        {
            string monthAndDay = _date.ToString("MMMM") + ", " + _date.Day;
            return monthAndDay;
        }

        public DateTime GetChangeDate(int numberDay)
        {
            _date = new DateTime(_date.Year, _date.Month, numberDay);
            return _date;
        }

        public void NextMonth()
        {
            try
            {
                _date = new DateTime(_date.Year, _date.Month + 1, 1);
            }
            catch(ArgumentOutOfRangeException) 
            {
                _date = new DateTime(_date.Year + 1, 1, 1);        
            }

            if (_date.Month == DateTime.Now.Month && _date.Year == DateTime.Now.Year)
            {
                _date = DateTime.Now;
            }
        }

        public void PreviousMonth()
        {
            try
            {
                _date = new DateTime(_date.Year, _date.Month - 1, DateTime.DaysInMonth(_date.Year, _date.Month - 1));
            }
            catch (ArgumentOutOfRangeException)
            {
                _date = new DateTime(_date.Year - 1, 12, DateTime.DaysInMonth(_date.Year - 1, 12));
            }

            if(_date.Month == DateTime.Now.Month && _date.Year == DateTime.Now.Year)
            {
                _date = DateTime.Now;
            }
        }

        public void Today()
        {
            _date = DateTime.Now;
        }        

        public int GetStartDayMonth() 
        {
            DateTime tempDate = _date;
            int numeberDay = 0;
            bool cheak = false;

            while(!cheak)
            {
                try
                {
                    tempDate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day - 1);
                }
                catch(ArgumentOutOfRangeException)
                {
                    cheak = true;
                }
            }
            numeberDay = (int)tempDate.DayOfWeek;
            if (numeberDay == 0) { numeberDay = 6; } //чтобы нулевой день был Пн, а не Вс
            else { numeberDay--; }

            return numeberDay;
        }

        public int GetAmountDayMonth()
        {
            return DateTime.DaysInMonth(_date.Year, _date.Month);
        }
    }
}
