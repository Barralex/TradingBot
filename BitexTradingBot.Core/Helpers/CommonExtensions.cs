using System;

namespace BitexTradingBot.Core.Helpers
{
    public static class CommonExtensions
    {
        public static double CalculatePercentDiference(this double orderPrice, double actualPrice)
        {
            return ((actualPrice - orderPrice) / orderPrice) * 100;
        }

        public static double CalculateProfitPrice(this double orderPrice, double minimum, double maximum)
        {
            var profitAverage = new Random().NextDouble() * (maximum - minimum) + minimum;
            return orderPrice - (orderPrice * profitAverage);
        }

        public static bool IsOrderType(this string orderType, string orderTypeToCompare)
        {
            return orderType == orderTypeToCompare;
        }
    }
}