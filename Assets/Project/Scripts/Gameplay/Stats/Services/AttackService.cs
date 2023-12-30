using System;
using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public class AttackService : IAttackService
    {
        private readonly IStatsWasteService _statsWasteService;

        public AttackService(IStatsWasteService statsWasteService)
        {
            _statsWasteService = statsWasteService;
        }

        public void Attack(CharacterStats statsAttacking, CharacterStats statsDefending)
        {
            float damage = statsAttacking.MainStats.damage - statsDefending.MainStats.defense;
            damage = Math.Clamp(damage, 0, float.MaxValue);
            
            _statsWasteService.WasteHealth(statsDefending, damage);
            _statsWasteService.WasteStamina(statsAttacking, statsAttacking.MainStats.staminaCost);

            TryDead(statsDefending);
        }

        private void TryDead(CharacterStats statsCharacter)
        {
            if (statsCharacter.CurrentStats.Health != 0)
                return;
            
            Debug.Log("Enemy dead.");
        }
    }
}