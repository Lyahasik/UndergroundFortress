using System;
using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Character;

using Random = UnityEngine.Random;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class AttackService : IAttackService
    {
        private const float OneIntegerValue = 1f;
        
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
                return;
            
            float damage = statsAttacking.MainStats[StatType.Damage] - statsDefending.MainStats[StatType.Defense];
            damage = Math.Clamp(damage, 0, float.MaxValue);

            TryBreakThrough(statsAttacking, statsDefending, ref damage);
            TryApplyCrit(statsAttacking, statsDefending, ref damage);
            
            _statsWasteService.WasteHealth(statsDefending, (int) damage);
            _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats[StatType.StaminaCost]);

            if (TryDead(statsDefending))
                return;

            TryStun(statsAttacking, dataDefending);
        }

        private bool TryHit(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            float probabilityMiss
                = statsDefending.MainStats[StatType.Dodge] - statsAttacking.MainStats[StatType.Accuracy];
            probabilityMiss = Math.Clamp(probabilityMiss, 0, statsDefending.MainStats[StatType.Dodge]);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            return result >= probabilityMiss;
        }

        private void TryBreakThrough(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityBlock =
                statsDefending.MainStats[StatType.Block] - statsAttacking.MainStats[StatType.BreakThrough];
            probabilityBlock = Math.Clamp(probabilityBlock, 0, statsDefending.MainStats[StatType.Block]);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            if (result < probabilityBlock)
                damage -= damage * statsDefending.MainStats[StatType.BlockAttackDamage];
        }

        private void TryApplyCrit(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityCrit =
                statsAttacking.MainStats[StatType.Crit] - statsDefending.MainStats[StatType.Parry];
            probabilityCrit = Math.Clamp(probabilityCrit, 0, statsAttacking.MainStats[StatType.Crit]);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            if (result >= probabilityCrit)
                damage *= OneIntegerValue + statsAttacking.MainStats[StatType.CritDamage];
        }

        private void TryStun(CharacterStats statsAttacking, CharacterData dataDefending)
        {
            float probabilityStun =
                statsAttacking.MainStats[StatType.Stun] - dataDefending.Stats.MainStats[StatType.Strength];
            probabilityStun = Math.Clamp(probabilityStun, 0, statsAttacking.MainStats[StatType.Stun]);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);
            
            if (result < probabilityStun)
                dataDefending.Stunned.Activate(statsAttacking.MainStats[StatType.StunDuration]);
        }

        private bool TryDead(CharacterStats statsCharacter)
        {
            if (statsCharacter.CurrentStats.Health != 0)
                return false;
            
            Debug.Log("Enemy dead.");
            return true;
        }
    }
}