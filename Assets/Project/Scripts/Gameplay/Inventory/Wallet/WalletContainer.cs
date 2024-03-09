using System;

namespace UndergroundFortress.Gameplay.Inventory.Wallet
{
    public class WalletContainer
    {
        private int _money;
        private int _realMoney;

        public int Money => _money;
        public int RealMoney => _realMoney;

        public WalletContainer(int money, int realMoney)
        {
            _money = money;
            _realMoney = realMoney;
        }

        public void AddMoney(in int value) => 
            _money += value;

        public void RemoveMoney(in int value) => 
            _money = Math.Max(_money - value, 0);

        public void AddRealMoney(in int value) => 
            _realMoney += value;

        public void RemoveRealMoney(in int value) => 
            _realMoney = Math.Max(_realMoney - value, 0);
    }
}