using UnityEngine;

using UndergroundFortress.Core.Services.StaticData;
using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Testing
{
    public class MainStatsView : MonoBehaviour
    {
        [SerializeField] private StatView health;
        [SerializeField] private StatView healthRecoveryRate;
        
        [Space]
        [SerializeField] private StatView stamina;
        [SerializeField] private StatView staminaRecoveryRate;
        [SerializeField] private StatView staminaCost;
        
        [Space]
        [SerializeField] private StatView damage;
        
        [Space]
        [SerializeField] private StatView defence;
        
        [Space]
        [SerializeField] private StatView dodge;
        [SerializeField] private StatView accuracy;
        
        [Space]
        [SerializeField] private StatView crit;
        [SerializeField] private StatView parry;
        [SerializeField] private StatView critDamage;
        
        [Space]
        [SerializeField] private StatView block;
        [SerializeField] private StatView blockAttackDamage;
        [SerializeField] private StatView breakThrough;

        [Space]
        [SerializeField] private StatView stun;
        [SerializeField] private StatView stunDuration;
        [SerializeField] private StatView strength;

        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void UpdateValues(CharacterStats stats)
        {
            SetStat(health, stats, StatType.Health);
            SetStat(healthRecoveryRate, stats, StatType.HealthRecoveryRate);
            
            SetStat(stamina, stats, StatType.Stamina);
            SetStat(staminaRecoveryRate, stats, StatType.StaminaRecoveryRate);
            SetStat(staminaCost, stats, StatType.StaminaCost);
            
            SetStat(damage, stats, StatType.Damage);
            
            SetStat(defence, stats, StatType.Defense);
            
            SetStat(dodge, stats, StatType.Dodge);
            SetStat(accuracy, stats, StatType.Accuracy);
            
            SetStat(crit, stats, StatType.Crit);
            SetStat(parry, stats, StatType.Parry);
            SetStat(critDamage, stats, StatType.CritDamage);
            
            SetStat(block, stats, StatType.Block);
            SetStat(blockAttackDamage, stats, StatType.BlockAttackDamage);
            SetStat(breakThrough, stats, StatType.BreakThrough);
            
            SetStat(stun, stats, StatType.Stun);
            SetStat(stunDuration, stats, StatType.StunDuration);
            SetStat(strength, stats, StatType.Strength);
        }

        private void SetStat(StatView statView, CharacterStats stats, StatType statType)
        {
            statView.SetValues(_staticDataService.GetStatByType(statType).keyName,
                _staticDataService.GetStatByType(statType).icon,
                QualityType.Grey,
                stats.MainStats[statType]);
        }
    }
}