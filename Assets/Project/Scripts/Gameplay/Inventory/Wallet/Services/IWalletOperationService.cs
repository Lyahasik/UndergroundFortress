using UndergroundFortress.Core.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Wallet.Services
{
    public interface IWalletOperationService : IService
    {
        public int Money1 { get; }
        public int Money2 { get; }
        
        public void Initialize();
        public void AddMoney(in MoneyType moneyType, in int value);
        public void AddMoney1(in int value);
        public void RemoveMoney(MoneyType moneyType, in int value);
        public void RemoveMoney1(in int value);
        public void AddMoney2(in int value);
        public void RemoveMoney2(in int value);
        public bool IsEnoughMoney(MoneyType moneyType, in int value);
    }
}