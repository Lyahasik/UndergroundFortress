using System;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Character;

using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class AttackService : IAttackService
    {
        private readonly IStatsWasteService _statsWasteService;

        public AttackService(IStatsWasteService statsWasteService)
        {
            _statsWasteService = statsWasteService;
        }

        public void Attack(CharacterData dataAttacking, CharacterData dataDefending)
        {
            CharacterStats statsAttacking = dataAttacking.Stats;
            CharacterStats statsDefending = dataDefending.Stats;

            if (!TryHit(statsAttacking, statsDefending))
            {
                _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats[StatType.StaminaCost]);
                dataAttacking.AttackEffect(StatType.Dodge);
                dataDefending.TakeHitEffect(StatType.Dodge);
                
                return;
            }
            
            float damage = statsAttacking.MainStats[StatType.Damage] - statsDefending.MainStats[StatType.Defense];
            damage = Math.Clamp(damage, 0, float.MaxValue);

            if (TryBreakThrough(statsAttacking, statsDefending, ref damage))
                dataDefending.TakeHitEffect(StatType.Damage);
            else
                dataDefending.TakeHitEffect(StatType.Block);
                
            if (TryApplyCrit(statsAttacking, statsDefending, ref damage))
                dataAttacking.AttackEffect(StatType.Crit);
            
            _statsWasteService.WasteHealth(statsDefending, (int) damage);
            _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats[StatType.StaminaCost]);

            if (TryDead(dataDefending))
                return;

            if (TryStun(statsAttacking, dataDefending))
            {
                dataAttacking.AttackEffect(StatType.Stun);
                dataDefending.TakeHitEffect(StatType.Stun);
            }
            
            dataAttacking.AttackEffect(StatType.Damage);
        }

        private bool TryHit(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            float probabilityMiss
                = statsDefending.MainStats[StatType.Dodge] - statsAttacking.MainStats[StatType.Accuracy];
            probabilityMiss = Math.Clamp(probabilityMiss, 0, statsDefending.MainStats[StatType.Dodge]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            return result >= probabilityMiss;
        }

        private bool TryBreakThrough(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityBlock =
                statsDefending.MainStats[StatType.Block] - statsAttacking.MainStats[StatType.BreakThrough];
            probabilityBlock = Math.Clamp(probabilityBlock, 0, statsDefending.MainStats[StatType.Block]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityBlock)
                damage -= damage * (statsAttacking.MainStats[StatType.BlockAttackDamage] / ConstantValues.MAX_PERCENTAGE);

            return result >= probabilityBlock;
        }

        private bool TryApplyCrit(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityCrit =
                statsAttacking.MainStats[StatType.Crit] - statsDefending.MainStats[StatType.Parry];
            probabilityCrit = Math.Clamp(probabilityCrit, 0, statsAttacking.MainStats[StatType.Crit]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityCrit)
                damage += damage * (statsAttacking.MainStats[StatType.CritDamage] / ConstantValues.MAX_PERCENTAGE);

            return result < probabilityCrit;
        }

        private bool TryStun(CharacterStats statsAttacking, CharacterData dataDefending)
        {
            float probabilityStun =
                statsAttacking.MainStats[StatType.Stun] - dataDefending.Stats.MainStats[StatType.Strength];
            probabilityStun = Math.Clamp(probabilityStun, 0, statsAttacking.MainStats[StatType.Stun]);

            float result = Random.Range(0f, ConstantValues.MAX_PERCENTAGE);

            if (result < probabilityStun)
                dataDefending.ActivateStun(statsAttacking.MainStats[StatType.StunDuration]);

            return result < probabilityStun;
        }

        private bool TryDead(CharacterData dataDefending)
        {
            if (dataDefending.Stats.CurrentStats.Health != 0)
                return false;

            dataDefending.StartDead();
            return true;
        }
    }
}