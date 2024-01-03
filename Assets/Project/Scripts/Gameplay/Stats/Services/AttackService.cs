using System;
using UnityEngine;

using UndergroundFortress.Scripts.Constants;
using UndergroundFortress.Scripts.Gameplay.Character;

using Random = UnityEngine.Random;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
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
            
            float damage = statsAttacking.MainStats.damage - statsDefending.MainStats.defense;
            damage = Math.Clamp(damage, 0, float.MaxValue);

            TryBreakThrough(statsAttacking, statsDefending, ref damage);
            TryApplyCrit(statsAttacking, statsDefending, ref damage);
            
            _statsWasteService.WasteHealth(statsDefending, (int) damage);
            _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats.staminaCost);

            if (TryDead(statsDefending))
                return;

            TryStun(statsAttacking, dataDefending);
        }

        private bool TryHit(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            float probabilityMiss = statsDefending.MainStats.dodge - statsAttacking.MainStats.accuracy;
            probabilityMiss = Math.Clamp(probabilityMiss, 0, statsDefending.MainStats.dodge);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            return result >= probabilityMiss;
        }

        private void TryBreakThrough(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityBlock = statsDefending.MainStats.block - statsAttacking.MainStats.breakThrough;
            probabilityBlock = Math.Clamp(probabilityBlock, 0, statsDefending.MainStats.block);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            if (result < probabilityBlock)
                damage -= damage * statsDefending.MainStats.blockAttackDamage;
        }

        private void TryApplyCrit(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityCrit = statsAttacking.MainStats.crit - statsDefending.MainStats.parry;
            probabilityCrit = Math.Clamp(probabilityCrit, 0, statsAttacking.MainStats.crit);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            if (result >= probabilityCrit)
                damage *= OneIntegerValue + statsAttacking.MainStats.critDamage;
        }

        private void TryStun(CharacterStats statsAttacking, CharacterData dataDefending)
        {
            float probabilityStun = statsAttacking.MainStats.stun - dataDefending.Stats.MainStats.strength;
            probabilityStun = Math.Clamp(probabilityStun, 0, statsAttacking.MainStats.stun);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);
            
            if (result < probabilityStun)
                dataDefending.Stunned.Activate(statsAttacking.MainStats.stunDuration);
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