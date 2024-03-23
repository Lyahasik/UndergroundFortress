using System;

using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Gameplay.Inventory.Wallet.Services
{
    public interface IWalletOperationService : IService
    {
        public int Money { get; }
        public int RealMoney { get; }
        
        public void Initialize();
        public void AddMoney1(in int value);
        public void RemoveMoney1(in int value);
        public void AddMoney2(in int value);
        public void RemoveMoney2(in int value);
        public bool IsEnoughMoney(in int value);
        public bool IsEnoughRealMoney(in int value);
    }
}