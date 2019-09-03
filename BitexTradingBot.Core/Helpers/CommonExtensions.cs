using System;

namespace BitexTradingBot.Core.Helpers
{
    public static class CommonExtensions
    {
        public static double CalculateOrderPrice(this double orderPrice, double minimum, double maximum)
        {
            var profitAverage = new Random().NextDouble() * (maximum - minimum) + minimum;
            return orderPrice - (orderPrice * profitAverage);
        }
    }
}
