using System.Collections.Generic;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character.Services
{
    public class ProcessingPlayerStatsService : IProcessingPlayerStatsService, IWritingProgress
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        
        private CharacterStats _playerStats;
        private Dictionary<StatType, float> _basePlayerStats;
        private Dictionary<StatType, float> _progressPlayerStats;
        
        public CharacterStats PlayerStats => _playerStats;

        public ProcessingPlayerStatsService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            LoadingBaseStats();

            Register(_progressProviderService);
        }
            
        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            LoadMainStats(progress);
            
            _playerStats = new CharacterStats();
            _playerStats.Initialize();
            UpdateStats();
            _playerStats.UpdateCurrentStats();
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            
        }

        public void UpStat(in StatType type, in float value)
        {
            _progressPlayerStats[type] += value;
            UpdateStats();
        }

        public void DownStat(in StatType type, in float value)
        {
            _progressPlayerStats[type] -= value;
            UpdateStats();
        }

        public void UpHealth(in float value)
        {
            float oldValue = _progressPlayerStats[StatType.Health];
            _progressPlayerStats[StatType.Health] += value;
            RecalculateCurrentHealth(oldValue);
            
            UpdateStats();
        }

        public void DownHealth(in float value)
        {
            float oldValue = _progressPlayerStats[StatType.Health];
            _progressPlayerStats[StatType.Health] -= value;
            RecalculateCurrentHealth(oldValue);
            
            UpdateStats();
        }

        private void LoadingBaseStats()
        {
            _basePlayerStats = new Dictionary<StatType, float>();
            ParseBaseStats(_staticDataService.ForPlayer());
        }

        private void ParseBaseStats(CharacterStaticData characterStaticData)
        {
            _basePlayerStats = new Dictionary<StatType, float>();
            
            _basePlayerStats.Add(StatType.Health, characterStaticData.health);
            _basePlayerStats.Add(StatType.HealthRecoveryRate, characterStaticData.healthRecoveryRate);
            
            _basePlayerStats.Add(StatType.Stamina, characterStaticData.stamina);
            _basePlayerStats.Add(StatType.StaminaRecoveryRate, characterStaticData.staminaRecoveryRate);
            _basePlayerStats.Add(StatType.StaminaCost, characterStaticData.staminaCost);
            
            _basePlayerStats.Add(StatType.Damage, characterStaticData.damage);
            
            _basePlayerStats.Add(StatType.Defense, characterStaticData.defense);
            
            _basePlayerStats.Add(StatType.Dodge, characterStaticData.dodge);
            _basePlayerStats.Add(StatType.Accuracy, characterStaticData.accuracy);
            
            _basePlayerStats.Add(StatType.Crit, characterStaticData.crit);
            _basePlayerStats.Add(StatType.Parry, characterStaticData.parry);
            _basePlayerStats.Add(StatType.CritDamage, characterStaticData.critDamage);
            
            _basePlayerStats.Add(StatType.Block, characterStaticData.block);
            _basePlayerStats.Add(StatType.BreakThrough, characterStaticData.breakThrough);
            _basePlayerStats.Add(StatType.BlockAttackDamage, characterStaticData.blockAttackDamage);
            
            _basePlayerStats.Add(StatType.Stun, characterStaticData.stun);
            _basePlayerStats.Add(StatType.Strength, characterStaticData.strength);
            _basePlayerStats.Add(StatType.StunDuration, characterStaticData.stunDuration);
        }

        private void LoadMainStats(ProgressData progressData) => 
            _progressPlayerStats = progressData.MainStats;

        private void UpdateStats()
        {
            foreach (KeyValuePair<StatType,float> keyValuePair in _basePlayerStats)
            {
                float newValue = _basePlayerStats[keyValuePair.Key] + _progressPlayerStats[keyValuePair.Key];
                _playerStats.MainStats[keyValuePair.Key] = newValue;
            }
        }

        private void RecalculateCurrentHealth(in float oldValue)
        {
            float healthRatio = oldValue / _progressPlayerStats[StatType.Health];
            _playerStats.SetCurrentHealth(_playerStats.CurrentStats.Health * healthRatio);
        }
    }
}