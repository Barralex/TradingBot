namespace BitexTradingBot.Core.DataAccess.DataBase.Enums
{
    public enum OrderStatusEnum
    {
        Open = 1,
        Finished,
        ManuallyCanceled,
        CanceledByPriceChange
    }
}
