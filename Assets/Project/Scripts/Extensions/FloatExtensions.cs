using System;

namespace UndergroundFortress.Extensions
{
    public static class FloatExtensions
    {
        public static string ToStringTime(this float value, string minuteLocale, string secondLocale)
        {
            int newValue = (int) MathF.Round(value);
            
            if (newValue > 59)
                return $"{ newValue / 60 }{minuteLocale} { newValue % 60 }{secondLocale}";

            return $"{ newValue }{secondLocale}";
        }
    }
}