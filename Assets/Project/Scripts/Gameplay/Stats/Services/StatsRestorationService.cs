using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Core.Services.Bonuses;
using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Dungeons.Services;

namespace UndergroundFortress.Gameplay.Stats.Services
{
    public class StatsRestorationService : IStatsRestorationService
    {
        private IProcessingBonusesService _processingBonusesService;
        private IProgressDungeonService _progressDungeonService;

        private List<CharacterStats> _statCharacters;

        public IProgressDungeonService ProgressDungeonService
        {
            set => _progressDungeonService = value;
        }

        public IProcessingBonusesService ProcessingBonusesService
        {
            set => _processingBonusesService = value;
        }

        public void Initialize()
        {
            _statCharacters = new();
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void AddStats(CharacterStats stats)
        {
            _statCharacters.Add(stats);
        }

        public void RemoveStats(CharacterStats stats)
        {
            _statCharacters.Remove(stats);
        }

        public void RestoreFullHealth(CharacterStats stats)
        {
            if (stats.CurrentStats.Health >= stats.MainStats[StatType.Health])
                return;

            stats.CurrentStats.Health = stats.MainStats[StatType.Health];
            stats.UpdateCurrent();
        }

        public void RestoreHealth(CharacterStats stats, int value)
        {
            stats.CurrentStats.Health = Math.Clamp(stats.CurrentStats.Health + value, 0, stats.MainStats[StatType.Health]);
            stats.UpdateCurrent();
        }

        public void RestoreStats()
        {
            foreach (CharacterStats stats in _statCharacters)
            {
                if (stats.IsFreeze)
                    continue;
                
                RestoreHealth(stats);
                RestoreStamina(stats);
            }
        }

        private void RestoreHealth(CharacterStats stats)
        {
            if (_progressDungeonService is { IsPause: false }
                || stats.CurrentStats.Health >= stats.MainStats[StatType.Health])
                return;

            float recoveryRate = stats.MainStats[StatType.HealthRecoveryRate];
            if (_processingBonusesService.IsBuffActivate(BonusType.DoubleRecoveryHealth))
                recoveryRate *= 2f;

            recoveryRate *= Time.deltaTime;
            stats.CurrentStats.Health = Math.Clamp(stats.CurrentStats.Health + recoveryRate, 0, stats.MainStats[StatType.Health]);
            stats.UpdateCurrent();
        }

        private void RestoreStamina(CharacterStats stats)
        {
            if (stats.CurrentStats.Stamina >= stats.MainStats[StatType.Stamina])
                return;

            stats.CurrentStats.Stamina =
                Math.Clamp(stats.CurrentStats.Stamina + stats.MainStats[StatType.StaminaRecoveryRate] * Time.deltaTime,
                    0, stats.MainStats[StatType.Stamina]);
            stats.UpdateCurrent();
        }
    }
}