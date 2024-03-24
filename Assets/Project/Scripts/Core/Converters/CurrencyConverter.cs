using UndergroundFortress.Constants;

namespace UndergroundFortress.Core.Converters
{
    public static class CurrencyConverter
    {
        public static int Money1ToPriceTime(int priceTime) => 
            priceTime * ConstantValues.PRICE_TIME_BY_MONEY1;
        
        public static int PriceTimeToMoney1(int priceTime)
        {
            int left = priceTime % ConstantValues.PRICE_TIME_BY_MONEY1 > 0 ? 1 : 0;
            
            return priceTime / ConstantValues.PRICE_TIME_BY_MONEY1 + left;
        }

        public static int Money2ToPriceTime(int priceTime) => 
            priceTime * ConstantValues.PRICE_TIME_BY_MONEY2;

        public static int PriceTimeToMoney2(int priceTime)
        {
            int left = priceTime % ConstantValues.PRICE_TIME_BY_MONEY2 > 0 ? 1 : 0;
            
            return priceTime / ConstantValues.PRICE_TIME_BY_MONEY2 + left;
        }
    }
}