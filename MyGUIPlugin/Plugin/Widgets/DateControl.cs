using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MyGUIPlugin
{
    public class DateControl
    {
        private ComboBox month;
        private ComboBox day;
        private NumericEdit year;

        public DateControl(ComboBox month, ComboBox day, Edit year)
        {
            this.month = month;
            int i = 1;
            foreach (String monthName in CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames)
            {
                if (!String.IsNullOrEmpty(monthName))
                {
                    month.addItem(monthName, i++);
                }
            }
            month.SelectedIndex = 0;
            month.EventComboChangePosition += new MyGUIEvent(month_EventComboChangePosition);
            this.day = day;
            this.year = new NumericEdit(year);
            this.year.MinValue = 1;
            this.year.MaxValue = 9999;
            this.year.AllowFloat = false;
            this.year.ValueChanged += new MyGUIEvent(year_ValueChanged);
            populateMonthDays();
        }

        public DateTime Date
        {
            get
            {
                return new DateTime(Year, Month, Day);
            }
            set
            {
                Month = value.Month;
                populateMonthDays();
                Day = value.Day;
                Year = value.Year;
            }
        }

        public int Month
        {
            get
            {
                return (int)month.SelectedItemData;
            }
            set
            {
                month.SelectedIndex = (uint)(value - 1);
            }
        }

        public int Day
        {
            get
            {
                if (day.SelectedItemData == null)
                {
                    return 1;
                }
                return (int)day.SelectedItemData;
            }
            set
            {
                uint dayValue = (uint)(value - 1);
                if(dayValue >= day.ItemCount)
                {
                    dayValue = 0;
                }
                day.SelectedIndex = dayValue;
            }
        }

        public int Year
        {
            get
            {
                return year.IntValue;
            }
            set
            {
                year.IntValue = value;
            }
        }

        private void populateMonthDays()
        {
            int currentDay = Day;
            day.removeAllItems();
            int theYear = year.IntValue;
            if (theYear == 0)
            {
                theYear = 1;
            }
            int dayCount = DateTime.DaysInMonth(theYear, Month);
            for (int i = 0; i < dayCount; i++)
            {
                day.addItem((i + 1).ToString(), i + 1);
            }
            Day = currentDay;
        }

        void month_EventComboChangePosition(Widget source, EventArgs e)
        {
            populateMonthDays();
        }

        void year_ValueChanged(Widget source, EventArgs e)
        {
            populateMonthDays();
        }
    }
}
