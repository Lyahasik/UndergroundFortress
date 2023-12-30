using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Scripts.Constants;
using UndergroundFortress.Scripts.Gameplay.Character;

namespace UndergroundFortress.Scripts.Gameplay.Stats.Services
{
    public class StatsRestorationService : MonoBehaviour, IStatsRestorationService
    {
        private List<CharacterStats> _statCharacters;

        private float _nextRestoreTime;

        public void Initialize()
        {
            _statCharacters = new();
        }

        private void Update()
        {
            RestoreStats();
        }

        public void AddStats(CharacterStats stats)
        {
            _statCharacters.Add(stats);
        }

        public void RemoveStats(CharacterStats stats)
        {
            _statCharacters.Remove(stats);
        }

        private void RestoreStats()
        {
            if (_nextRestoreTime > Time.time)
                return;
            
            foreach (CharacterStats stats in _statCharacters)
            {
                RestoreHealth(stats);
                RestoreStamina(stats);
            }

            _nextRestoreTime = Time.time + ConstantValues.DELAY_RESTORE_STATS;
        }

        private void RestoreHealth(CharacterStats stats)
        {
            if (stats.CurrentStats.Health >= stats.MainStats.health)
                return;

            stats.CurrentStats.Health =
                Math.Clamp(stats.CurrentStats.Health + stats.MainStats.healthRecoveryRate, 0, stats.MainStats.health);
            stats.Update();
        }

        private void RestoreStamina(CharacterStats stats)
        {
            if (stats.CurrentStats.Stamina >= stats.MainStats.stamina)
                return;

            stats.CurrentStats.Stamina =
                Math.Clamp(stats.CurrentStats.Stamina + stats.MainStats.staminaRecoveryRate, 0, stats.MainStats.stamina);
            stats.Update();
        }
    }
}