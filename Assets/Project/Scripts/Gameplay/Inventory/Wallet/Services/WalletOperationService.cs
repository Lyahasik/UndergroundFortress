using System;

namespace UndergroundFortress.Gameplay.Inventory.Wallet.Services
{
    public class WalletOperationService : IWalletOperationService
    {
        private WalletContainer _walletContainer;

        public int Money => _walletContainer.Money;
        public int RealMoney => _walletContainer.RealMoney;
        
        public event Action<int> OnUpdateMoney;
        public event Action<int> OnUpdateRealMoney;

        public void Initialize()
        {
            _walletContainer = new WalletContainer(1000, 100);
        }

        public void AddMoney(in int value)
        {
            _walletContainer.AddMoney(value);
            OnUpdateMoney?.Invoke(_walletContainer.Money);
        }

        public void RemoveMoney(in int value)
        {
            _walletContainer.RemoveMoney(value);
            OnUpdateMoney?.Invoke(_walletContainer.Money);
        }

        public void AddRealMoney(in int value)
        {
            _walletContainer.AddRealMoney(value);
            OnUpdateRealMoney?.Invoke(_walletContainer.Money);
        }

        public void RemoveRealMoney(in int value)
        {
            _walletContainer.RemoveRealMoney(value);
            OnUpdateRealMoney?.Invoke(_walletContainer.Money);
        }

        public bool IsEnoughMoney(in int value) => 
            _walletContainer.Money >= value;

        public bool IsEnoughRealMoney(in int value) => 
            _walletContainer.RealMoney >= value;
    }
}