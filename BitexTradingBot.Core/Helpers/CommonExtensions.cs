using BitexTradingBot.Core.DataAccess.DataBase.Enums;
using System;

namespace BitexTradingBot.Core.Helpers
{
    public static class CommonExtensions
    {
        public static double CalculatePercentDiference(this double orderPrice, double actualPrice)
        {
            return ((actualPrice - orderPrice) / orderPrice) * 100;
        }

        public static double CalculateProfitMargin(this double orderPrice, double minimum, double maximum)
        {
            var profitAverage = new Random().NextDouble() * (maximum - minimum) + minimum;
            return orderPrice * profitAverage;
        }

        public static bool IsOrderType(this string orderType, string orderTypeToCompare)
        {
            return orderType == orderTypeToCompare;
        }

        public static OrderStatusEnum BitexStatusToDatabase(this string status)
        {
            OrderStatusEnum result = OrderStatusEnum.Open;

            switch (status)
            {
                case "cancelled":
                    result = OrderStatusEnum.ManuallyCanceled;
                    break;

                case "completed":
                    result = OrderStatusEnum.Finished;
                    break;
            }
            return result;
        }
    }
}