using System;

using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Gameplay.Inventory.Wallet.Services
{
    public interface IWalletOperationService : IService
    {
        public int Money { get; }
        public int RealMoney { get; }
        
        public event Action<int> OnUpdateMoney;
        public event Action<int> OnUpdateRealMoney;
        
        public void Initialize();
        public void AddMoney(in int value);
        public void RemoveMoney(in int value);
        public void AddRealMoney(in int value);
        public void RemoveRealMoney(in int value);
        public bool IsEnoughMoney(in int value);
        public bool IsEnoughRealMoney(in int value);
    }
}