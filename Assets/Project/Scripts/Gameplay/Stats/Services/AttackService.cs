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

        public void Attack(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            if (!TryHit(statsAttacking, statsDefending))
                return;
            
            float damage = statsAttacking.MainStats.damage - statsDefending.MainStats.defense;
            damage = Math.Clamp(damage, 0, float.MaxValue);

            TryApplyCrit(statsAttacking, statsDefending, ref damage);
            
            _statsWasteService.WasteHealth(statsDefending, (int) damage);
            _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats.staminaCost);

            TryDead(statsDefending);
        }

        private bool TryHit(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            float probabilityMiss = statsDefending.MainStats.dodge - statsAttacking.MainStats.accuracy;
            probabilityMiss = Math.Clamp(probabilityMiss, 0, statsDefending.MainStats.dodge);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            return result >= probabilityMiss;
        }

        private void TryApplyCrit(CharacterStats statsAttacking, CharacterStats statsDefending, ref float damage)
        {
            float probabilityCrit = statsAttacking.MainStats.crit - statsDefending.MainStats.parry;
            probabilityCrit = Math.Clamp(probabilityCrit, 0, statsAttacking.MainStats.crit);

            float result = Random.Range(0f, ConstantValues.MAX_PROBABILITY);

            if (result >= probabilityCrit)
                damage *= OneIntegerValue + statsAttacking.MainStats.critDamage;
        }

        private void TryDead(CharacterStats statsCharacter)
        {
            if (statsCharacter.CurrentStats.Health != 0)
                return;
            
            Debug.Log("Enemy dead.");
        }
    }
}