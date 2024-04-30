using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Update;
using UndergroundFortress.Gameplay.Inventory.Services;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Tutorial.Services;
using UndergroundFortress.UI.Inventory;
using UndergroundFortress.UI.MainMenu;

namespace UndergroundFortress.Core.Services.Rewards
{
    public class AccumulationRewardsService : IAccumulationRewardsService, IWritingProgress, IUpdating
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        private IInventoryService _inventoryService;
        private MainMenuView _mainMenuView;

        private PlayerLevelData _levelData;
        private RewardsData _rewardsData;
        private HashSet<int> _tutorialStages;

        private RewardsStaticData _rewardsStaticData;
        private int _maxRewardNumberCoins;
        private float _delayIncreaseReward;

        public AccumulationRewardsService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize(IInventoryService inventoryService, MainMenuView mainMenuView)
        {
            _inventoryService = inventoryService;
            _mainMenuView = mainMenuView;
            
            _rewardsStaticData = _staticDataService.ForRewards();
            _delayIncreaseReward = _rewardsStaticData.delayAccrualSeconds;
                
            Register(_progressProviderService);
        }

        public void Update()
        {
            TryIncreaseReward();
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _levelData = progress.LevelData;
            _rewardsData = progress.RewardsData;
            _tutorialStages = progress.TutorialStages;

            UpdateProgress(progress);
            CalculateOfflineTime();
        }

        public void UpdateProgress(ProgressData progress)
        {
            _maxRewardNumberCoins
                = (int) ((_rewardsStaticData.maxAccumulationSeconds / _rewardsStaticData.delayAccrualSeconds)
                  * (_levelData.Level * _rewardsStaticData.numberCoinsByLevel));
        }

        public void WriteProgress()
        {
            _progressProviderService.WasChange();
        }

        public void ClaimRewards()
        {
            int numberCoins = _rewardsData.NumberCoins;
            
            _rewardsData.NumberCoins = 0;
            _rewardsData.LastCalculateTime = DateTime.Now.Ticks;
            _inventoryService.WalletOperationService.AddMoney(MoneyType.Money1, numberCoins);
        }

        private void CalculateOfflineTime()
        {
            if (_levelData.Level < ConstantValues.MIN_LEVEL)
                return;
            
            long deltaTicks = DateTime.Now.Ticks - _rewardsData.LastCalculateTime;
            TimeSpan timeSpan = new TimeSpan(deltaTicks);
             
            int offlineReward
                = (int) (timeSpan.TotalSeconds / _rewardsStaticData.delayAccrualSeconds * (_levelData.Level * _rewardsStaticData.numberCoinsByLevel));
                 
            _rewardsData.NumberCoins = Math.Clamp(_rewardsData.NumberCoins + offlineReward, 0, _maxRewardNumberCoins);
            _rewardsData.LastCalculateTime = DateTime.Now.Ticks;
             
            if (_rewardsData.NumberCoins >= _levelData.Level * _rewardsStaticData.numberToDisplayByLevel)
                _mainMenuView.ActivateAccumulatedRewardButton(_rewardsData);
        }

        private void TryIncreaseReward()
        {
            if (_rewardsData == null
                || _levelData.Level < ConstantValues.MIN_LEVEL)
                return;
            
            _delayIncreaseReward -= Time.deltaTime;
            
            if (_delayIncreaseReward > 0f)
                return;

            _rewardsData.NumberCoins = Math.Clamp(
                _rewardsData.NumberCoins + _levelData.Level * _rewardsStaticData.numberCoinsByLevel,
                0,
                _maxRewardNumberCoins);
            _rewardsData.LastCalculateTime = DateTime.Now.Ticks;
            WriteProgress();

            if (_tutorialStages.Contains((int) TutorialStageType.FirstEquipmentPotion)
                && _rewardsData.NumberCoins >= _levelData.Level * _rewardsStaticData.numberToDisplayByLevel)
                _mainMenuView.ActivateAccumulatedRewardButton(_rewardsData);
            
            _delayIncreaseReward = _rewardsStaticData.delayAccrualSeconds;
        }
    }
}