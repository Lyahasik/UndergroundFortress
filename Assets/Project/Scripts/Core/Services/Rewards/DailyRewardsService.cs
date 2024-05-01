using System;
using System.Collections.Generic;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Wallet.Services;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.Information.Services;
using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Core.Services.Rewards
{
    public class DailyRewardsService : IDailyRewardsService, IWritingProgress
    {
        private IStaticDataService _staticDataService;
        private IProgressProviderService _progressProviderService;
        private IItemsGeneratorService _itemsGeneratorService;
        private IWalletOperationService _walletOperationService;
        private IInformationService _informationService;
        
        private RewardsData _rewardsData;
        private PlayerLevelData _levelData;
        private HashSet<int> _tutorialStages;

        private List<MoneyNumberData> _rewardMoneys;
        private List<ItemNumberData> _rewardItems;

        public DailyRewardsService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService,
            IWalletOperationService walletOperationService,
            IInformationService informationService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _itemsGeneratorService = itemsGeneratorService;
            _walletOperationService = walletOperationService;
            _informationService = informationService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _rewardsData = progress.RewardsData;
            _levelData = progress.LevelData;
            _tutorialStages = progress.TutorialStages;

            if (_tutorialStages.Contains((int) TutorialStageType.FirstEquipmentPotion)
                && (_rewardsData.LastDateAward == null
                    || _rewardsData.LastDateAward.IsNewDay(DateTime.Now)))
                PresentAward();
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SaveProgress();
        }

        private void PresentAward()
        {
            var dailyRewards = _staticDataService.ForRewards().dailyRewards;
            if (_rewardsData.LastAwardId >= dailyRewards.Count)
                return;
            
            _rewardMoneys = dailyRewards[_rewardsData.LastAwardId].moneys;
            _rewardItems = dailyRewards[_rewardsData.LastAwardId].items;

            _informationService.ShowDailyRewards(dailyRewards, _rewardsData);
        }

        public void ClaimReward()
        {
            _rewardsData.LastAwardId++;
            _rewardsData.LastDateAward ??= new RewardDate();
            _rewardsData.LastDateAward.UpdateValues();
            
            _rewardMoneys
                .ForEach(data => _walletOperationService.AddMoney(data.moneyType, data.number));
            _rewardItems
                .ForEach(data =>
                {
                    if (_staticDataService.GetItemById(data.itemData.id).type.IsEquipment())
                        _itemsGeneratorService.GenerateEquipments(
                            data.itemData.id,
                            data.number,
                            _levelData.Level,
                            _staticDataService.GetEquipmentById(data.itemData.id).additionalStatType,
                            data.qualityType);
                    else
                        _itemsGeneratorService.GenerateResourcesById(data.itemData.id, data.number);
                });
        }
    }
}