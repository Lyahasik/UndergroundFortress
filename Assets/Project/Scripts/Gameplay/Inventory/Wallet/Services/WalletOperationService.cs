using System;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Localization;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.Inventory.Wallet.Services
{
    public class WalletOperationService : IWalletOperationService, IWritingProgress
    {
        private ILocalizationService _localizationService;
        private IProgressProviderService _progressProviderService;
        private IInformationService _informationService;

        private WalletData _walletData;

        public int Money1 => _walletData.Money1;

        public int Money2 => _walletData.Money2;

        public void Construct(ILocalizationService localizationService,
            IProgressProviderService progressProviderService,
            IInformationService informationService)
        {
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
            _informationService = informationService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
        }

        public void AddMoney(in MoneyType moneyType, in int value)
        {
            switch (moneyType)
            {
                case MoneyType.Money1:
                    AddMoney1(value);
                    break;
                case MoneyType.Money2:
                    AddMoney2(value);
                    break;
            }
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress) => 
            _walletData = progress.Wallet;

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress() => 
            _progressProviderService.SaveProgress();

        public void AddMoney1(in int value)
        {
            _walletData.Money1 += value;
            WriteProgress();
        }

        public void RemoveMoney(MoneyType moneyType, in int value)
        {
            switch (moneyType)
            {
                case MoneyType.Money1:
                    RemoveMoney1(value);
                    break;
                case MoneyType.Money2:
                    RemoveMoney2(value);
                    break;
            }
        }

        public bool IsEnoughMoney(MoneyType moneyType, in int value, bool isShowingWarning = true)
        {
            bool isEnough = false;
            
            switch (moneyType)
            {
                case MoneyType.Money1:
                    isEnough = _walletData.Money1 >= value;
                    if (isShowingWarning && !isEnough)
                        _informationService.ShowWarning(_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_NOT_COINS));
                    break;
                case MoneyType.Money2:
                    isEnough = _walletData.Money2 >= value;
                    if (isShowingWarning && !isEnough)
                        _informationService.ShowWarning(_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_NOT_DIAMONDS));
                    break;
                case MoneyType.Ads:
                    return true;
                //TODO delete
                case MoneyType.Money3:
                    return true;
                    break;
            }

            return isEnough;
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
    }
}