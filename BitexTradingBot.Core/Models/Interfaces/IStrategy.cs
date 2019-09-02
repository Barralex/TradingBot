﻿using System.Threading.Tasks;

namespace BitexTradingBot.Core.Interfaces
{
    public interface IStrategy
    {
        Task BuyAtMarket();
        Task SellAtMarket();
        Task OnStarted();
    }
}