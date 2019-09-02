﻿namespace BitexTradingBot.Core.Interfaces
{
    public interface IWebJobConfiguration
    {
        string BitexApiUrl { get; set; }
        string BitexDefaultMarket { get; set; }
        string BitexApiKey { get; set; }
    }
}