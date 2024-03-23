using System;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;

namespace UndergroundFortress.Gameplay.Inventory.Wallet.Services
{
    public class WalletOperationService : IWalletOperationService, IWritingProgress
    {
        private IProgressProviderService _progressProviderService;

        private WalletData _walletData;

        public int Money => _walletData.Money1;
        public int RealMoney => _walletData.Money2;

        public void Construct(IProgressProviderService progressProviderService)
        {
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void AddMoney1(in int value)
        {
            _walletData.Money1 += value;
            WriteProgress();
        }

        public void RemoveMoney1(in int value)
        {
            _walletData.Money1 = Math.Max(_walletData.Money1 - value, 0);
            WriteProgress();
        }

        public void AddMoney2(in int value)
        {
            _walletData.Money2 += value;
            WriteProgress();
        }

        public void RemoveMoney2(in int value)
        {
            _walletData.Money2 = Math.Max(_walletData.Money2 - value, 0);
            WriteProgress();
        }

        public bool IsEnoughMoney(in int value) => 
            _walletData.Money1 >= value;

        public bool IsEnoughRealMoney(in int value) => 
            _walletData.Money2 >= value;

        public void WriteProgress() => 
            _progressProviderService.SaveProgress();

        public void ReadProgress(ProgressData progress) => 
            _walletData = progress.Wallet;
    }
}