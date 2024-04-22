using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Core.Update;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Items.Services;
using UndergroundFortress.Gameplay.Player.Level;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.UI.MainMenu;
using Random = UnityEngine.Random;

namespace UndergroundFortress.Core.Services.Bonuses
{
    public class ProcessingBonusesService : IProcessingBonusesService, IUpdating, IWritingProgress
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IItemsGeneratorService _itemsGeneratorService;

        private MainMenuView _mainMenuView;

        private PlayerLevelData _levelData;
        private Dictionary<BonusType, float> _buffsLifetime;

        private float _delayNextTimeShowOffer;
        private BonusData _currentOfferBonusData;

        public ProcessingBonusesService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IItemsGeneratorService itemsGeneratorService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _itemsGeneratorService = itemsGeneratorService;
        }

        public void Initialize(MainMenuView mainMenuView)
        {
            Register(_progressProviderService);

            _mainMenuView = mainMenuView;
            ActivateBuffs();

            _delayNextTimeShowOffer = _staticDataService.ForBonuses().sentenceOffers;
        }

        public void Update()
        {
            UpdateBuffsLifetime();
            TryActivateOffer();
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _levelData = progress.LevelData;
            _buffsLifetime = progress.BonusesLifetime;
            
            ActivateBuffs();
        }

        public void UpdateProgress(ProgressData progress) {}
        
        public void WriteProgress()
        {
            _progressProviderService.WasChange();
        }

        public void ShowBonusOffer(BonusType bonusType)
        {
            BonusData bonusData = _staticDataService
                .ForBonuses().bonusesData
                .Find(data => data.bonusType == bonusType);

            if (bonusType == BonusType.RandomEquipment)
                _mainMenuView.ActivateBonusOfferButton(bonusData, CreateEquipment);
            else
                _mainMenuView.ActivateBonusOfferButton(bonusData, BuffActivate);
            
            _currentOfferBonusData = bonusData;
        }

        public bool IsBuffActivate(BonusType bonusType) => 
            _buffsLifetime.ContainsKey(bonusType);

        public float GetBuffValue(BonusType bonusType)
        {
            if (!_buffsLifetime.ContainsKey(bonusType))
                return 0f;
            
            return _staticDataService
                .ForBonuses().bonusesData
                .Find(data => data.bonusType == bonusType)
                .value;
        }

        public float GetBuffLeftToLive(BonusType bonusType) => 
            _buffsLifetime.ContainsKey(bonusType) ? _buffsLifetime[bonusType] : 0f;

        private void TryActivateOffer()
        {
            if (_levelData.Level < _staticDataService.ForBonuses().unlockLevel
                || _mainMenuView == null
                || _buffsLifetime.Keys.Count >= ConstantValues.MAX_NUMBER_BUFFS_ACTIVE)
                return;
            
            _delayNextTimeShowOffer -= Time.deltaTime;
            
            if (_delayNextTimeShowOffer > 0f)
                return;

            var bonuses = _staticDataService
                .ForBonuses().bonusesData
                .Where(data => !_buffsLifetime.ContainsKey(data.bonusType))
                .ToList();
            
            int totalWeight = bonuses.Sum(data => data.probabilityWeight);
            int accident = Random.Range(0, totalWeight + 1);

            foreach (BonusData bonusData in bonuses)
            {
                accident -= bonusData.probabilityWeight;
                if (accident < 0)
                {
                    _currentOfferBonusData = bonusData;
                    break;
                }
            }
            
            ShowBonusOffer(_currentOfferBonusData.bonusType);
            
            _delayNextTimeShowOffer = _staticDataService.ForBonuses().sentenceOffers;
        }

        private void ActivateBuffs()
        {
            if (_mainMenuView == null
                || _buffsLifetime == null)
                return;
            
            foreach (KeyValuePair<BonusType, float> keyValuePair in _buffsLifetime)
                BuffActivate(keyValuePair.Key);
        }

        private void CreateEquipment()
        {
            var equipments = _staticDataService
                .ForEquipments()
                .Where(data => 
                    data.maxLevel >= _levelData.Level
                    && data.maxLevel - _levelData.Level < ConstantValues.DELTA_EQUIPMENT_MAX_LEVEL)
                .ToList();

            var equipmentData = equipments[Random.Range(0, equipments.Count)];

            _itemsGeneratorService
                .GenerateEquipment(equipmentData.id, _levelData.Level, qualityType: QualityType.Purple);
        }

        private void BuffActivate()
        {
            _mainMenuView.ShowBuff(this, _currentOfferBonusData);
            
            if (!_buffsLifetime.ContainsKey(_currentOfferBonusData.bonusType))
                _buffsLifetime.Add(_currentOfferBonusData.bonusType, _currentOfferBonusData.lifetimeBonus);
        }

        private void BuffActivate(BonusType bonusType)
        {
            _currentOfferBonusData = _staticDataService
                .ForBonuses().bonusesData
                .Find(data => data.bonusType == bonusType);
            
            BuffActivate();
        }

        private void UpdateBuffsLifetime()
        {
            if (_buffsLifetime == null)
                return;
            
            foreach (BonusType bonusType in _buffsLifetime.Keys.ToList()) 
                _buffsLifetime[bonusType] -= Time.deltaTime;
            
            foreach (BonusType bonusType in _buffsLifetime.Keys.ToList())
                if (_buffsLifetime[bonusType] <= 0f)
                    _buffsLifetime.Remove(bonusType);

            WriteProgress();
        }
    }
}