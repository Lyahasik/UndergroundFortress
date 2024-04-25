using System;

namespace UndergroundFortress.Core.Progress
{
    public class RewardDate
    {
        public int LastYearAward;
        public int LastMonthAward;
        public int LastDayAward;

        public void UpdateValues()
        {
            DateTime newDate = DateTime.Now;
            
            LastYearAward = newDate.Year;
            LastMonthAward = newDate.Month;
            LastDayAward = newDate.Day;
        }

        public bool IsNewDay(in DateTime dateTime) =>
            LastYearAward < dateTime.Year
            || LastMonthAward < dateTime.Month
            || LastDayAward < dateTime.Day;
    }
}