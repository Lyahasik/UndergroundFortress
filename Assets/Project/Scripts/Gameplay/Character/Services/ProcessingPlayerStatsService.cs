using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Progress;
using UndergroundFortress.Core.Services.Progress;
using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Gameplay.Stats.Services;

namespace UndergroundFortress.Gameplay.Character.Services
{
    public class ProcessingPlayerStatsService : IProcessingPlayerStatsService, IReadingProgress
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IProgressProviderService _progressProviderService;
        private readonly IStatsRestorationService _statsRestorationService;

        private CharacterStats _playerStats;
        private Dictionary<StatType, float> _basePlayerStats;
        private Dictionary<StatType, float> _equipmentPlayerStats;
        private Dictionary<StatType, float> _skillsPlayerStats;
        private Dictionary<SkillsType,HashSet<int>> _activeSkills;
        public Dictionary<StatType, ProgressSkillData> _progressSkills;

        public CharacterStats PlayerStats => _playerStats;

        public ProcessingPlayerStatsService(IStaticDataService staticDataService,
            IProgressProviderService progressProviderService,
            IStatsRestorationService statsRestorationService)
        {
            _staticDataService = staticDataService;
            _progressProviderService = progressProviderService;
            _statsRestorationService = statsRestorationService;
        }

        public void Initialize()
        {
            LoadingBaseStats();
            _skillsPlayerStats = new Dictionary<StatType, float>();

            Register(_progressProviderService);
            Debug.Log($"[{ GetType() }] initialize");
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
            
            _statsRestorationService.AddStats(_playerStats);
            
            UpdateStats();
            _playerStats.UpdateCurrentStats();
        }

        public void UpdateProgress(ProgressData progress)
        {
            UpdateStats();
            RecalculateCurrentHealth();
            RecalculateCurrentStamina();
        }

        public void SetStatEquipment(StatType type, float value)
        {
            _equipmentPlayerStats[type] = value;
            UpdateStats();
        }

        public void UpStatEquipment(StatType type, float value)
        {
            _equipmentPlayerStats[type] += value;
            UpdateStats();
        }

        public void DownStatEquipment(StatType type, float value)
        {
            _equipmentPlayerStats[type] -= value;
            UpdateStats();
        }

        public void UpHealthEquipment(in float value)
        {
            _equipmentPlayerStats[StatType.Health] += value;
            
            UpdateStats();
            RecalculateCurrentHealth();
        }

        public void DownHealthEquipment(in float value)
        {
            _equipmentPlayerStats[StatType.Health] -= value;
            
            UpdateStats();
            RecalculateCurrentHealth();
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

        private void LoadMainStats(ProgressData progressData)
        {
            _equipmentPlayerStats = progressData.MainStats;
            _activeSkills = progressData.ActiveSkills;
            _progressSkills = progressData.ProgressSkills;
            
            UpdateSkillStats();
        }

        private void UpdateStats()
        {
            UpdateSkillStats();
            
            foreach (KeyValuePair<StatType,float> keyValuePair in _basePlayerStats)
            {
                float newValue = _basePlayerStats[keyValuePair.Key] + _equipmentPlayerStats[keyValuePair.Key];
                _playerStats.MainStats[keyValuePair.Key] = newValue;
                
                if (_skillsPlayerStats.ContainsKey(keyValuePair.Key))
                    _playerStats.MainStats[keyValuePair.Key] += _skillsPlayerStats[keyValuePair.Key];
            }
        }

        private void UpdateSkillStats()
        {
            Dictionary<StatType, float> newStats = new Dictionary<StatType, float>();
            
            foreach (KeyValuePair<SkillsType,HashSet<int>> keyValuePair in _activeSkills)
            {
                foreach (int id in keyValuePair.Value)
                {
                    var skillData = _staticDataService
                        .ForSkillsByType(keyValuePair.Key).skillsData
                        .Find(data => data.id == id);

                    if (skillData.data.statType.IsPassiveProgress())
                        AddedPassiveStat(newStats, skillData);
                    else
                        AddedStat(newStats, skillData);
                }
            }

            _skillsPlayerStats = newStats;
        }

        private void AddedStat(Dictionary<StatType, float> newStats, SkillData skillData)
        {
            if (newStats.ContainsKey(skillData.data.statType))
                newStats[skillData.data.statType] += skillData.data.value;
            else
                newStats[skillData.data.statType] = skillData.data.value;
        }

        private void AddedPassiveStat(Dictionary<StatType, float> newStats, SkillData skillData) => 
            newStats[skillData.data.statType] = skillData.data.value * _progressSkills[skillData.data.statType].CurrentLevel;

        private void RecalculateCurrentHealth()
        {
            if (_playerStats.CurrentStats.Health > _playerStats.MainStats[StatType.Health])
                _playerStats.SetCurrentHealth(_playerStats.MainStats[StatType.Health]);
        }

        private void RecalculateCurrentStamina()
        {
            if (_playerStats.CurrentStats.Stamina > _playerStats.MainStats[StatType.Stamina])
                _playerStats.SetCurrentStamina(_playerStats.MainStats[StatType.Stamina]);
        }
    }
}