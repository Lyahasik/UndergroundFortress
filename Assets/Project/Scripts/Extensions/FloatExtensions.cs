using System;

namespace UndergroundFortress.Extensions
{
    public static class FloatExtensions
    {
        public static string ToStringTime(this float value)
        {
            int newValue = (int) MathF.Round(value);
            
            if (newValue > 59)
                return $"{ newValue / 60 }@m { newValue % 60 }@s";

            return $"{ newValue }@s";
        }
    }
}