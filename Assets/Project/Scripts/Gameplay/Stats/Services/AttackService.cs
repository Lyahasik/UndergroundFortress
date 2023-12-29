using System;
using UnityEngine;

using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public class AttackService : IAttackService
    {
        private CharacterStats _playerStats;
        private CharacterStats _enemyStats;

        public void Attack(CharacterStats characterStats, float damage)
        {
            MainStats mainStats = characterStats.MainStats;
            CurrentStats currentStats = characterStats.CurrentStats;
            
            currentStats.Health = Math.Clamp(currentStats.Health - damage, 0, mainStats.health);
            characterStats.Update();

            TryDead(characterStats);
        }

        private void TryDead(CharacterStats characterStats)
        {
            if (characterStats.CurrentStats.Health != 0)
                return;
            
            Debug.Log("Enemy dead.");
        }
    }
}