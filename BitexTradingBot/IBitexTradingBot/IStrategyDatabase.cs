using BitexTradingBot.Core.DataAccess.DataBase.Enums;
using BitexTradingBot.Core.DataAccess.DataBase.Models;
using BitexTradingBot.Core.Models;
using System.Threading.Tasks;

namespace BitexTradingBot
{
    public interface IStrategyDatabase
    {
        Task CreateTradingOrder(TradingOrderDetails attemptOrderData);

        Task UpdateTradingOrder(TradingOrderDetails attemptOrderData, Trading actualOrder);

        Trading GetLastTransaction();

        Task UpdateOrderStatus(Trading activeTrading, OrderStatusEnum statusChangeReason);
    }
}