using System;
using System.Collections.Generic;
using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Character
{
    public class CharacterStats
    {
        private Dictionary<StatType, float> _mainStats;
        private CurrentStats _currentStats;

        public event Action<CharacterStats> OnUpdateCurrent;

        public Dictionary<StatType, float> MainStats => _mainStats;
        public CurrentStats CurrentStats => _currentStats;

        public void Initialize()
        {
            _mainStats = new Dictionary<StatType, float>();
        }

        public void Initialize(CharacterStaticData characterStaticData)
        {
            Initialize();

            FillStats(characterStaticData);
        }

        public void UpdateCurrentStats()
        {
            _currentStats = new CurrentStats(_mainStats[StatType.Health], _mainStats[StatType.Stamina]);
            UpdateCurrent();
        }

        public void UpdateCurrent() => 
            OnUpdateCurrent?.Invoke(this);

        public void SetCurrentHealth(in float value)
        {
            _currentStats.Health = value;
            UpdateCurrent();
        }

        public void SetCurrentStamina(in float value)
        {
            _currentStats.Stamina = value;
            UpdateCurrent();
        }

        private void FillStats(CharacterStaticData characterStaticData)
        {
            _mainStats.Add(StatType.Health, characterStaticData.health);
            _mainStats.Add(StatType.HealthRecoveryRate, characterStaticData.healthRecoveryRate);
            
            _mainStats.Add(StatType.Stamina, characterStaticData.stamina);
            _mainStats.Add(StatType.StaminaRecoveryRate, characterStaticData.staminaRecoveryRate);
            _mainStats.Add(StatType.StaminaCost, characterStaticData.staminaCost);
            
            _mainStats.Add(StatType.Damage, characterStaticData.damage);
            
            _mainStats.Add(StatType.Defense, characterStaticData.defense);
            
            _mainStats.Add(StatType.Dodge, characterStaticData.dodge);
            _mainStats.Add(StatType.Accuracy, characterStaticData.accuracy);
            
            _mainStats.Add(StatType.Crit, characterStaticData.crit);
            _mainStats.Add(StatType.Parry, characterStaticData.parry);
            _mainStats.Add(StatType.CritDamage, characterStaticData.critDamage);
            
            _mainStats.Add(StatType.Block, characterStaticData.block);
            _mainStats.Add(StatType.BreakThrough, characterStaticData.breakThrough);
            _mainStats.Add(StatType.BlockAttackDamage, characterStaticData.blockAttackDamage);
            
            _mainStats.Add(StatType.Stun, characterStaticData.stun);
            _mainStats.Add(StatType.Strength, characterStaticData.strength);
            _mainStats.Add(StatType.StunDuration, characterStaticData.stunDuration);
            
            UpdateCurrentStats();
        }
    }
}