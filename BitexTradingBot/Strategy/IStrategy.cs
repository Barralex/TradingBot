using System.Threading.Tasks;

namespace BitexTradingBot
{
    public interface IStrategy
    {
        Task Start();
    }
}
