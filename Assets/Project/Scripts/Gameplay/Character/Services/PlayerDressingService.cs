using System;
using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Core.Services.Characters
{
    public class PlayerDressingService : IPlayerDressingService
    {
        private readonly IProcessingPlayerStatsService _processingPlayerStatsService;

        public PlayerDressingService(IProcessingPlayerStatsService processingPlayerStatsService)
        {
            _processingPlayerStatsService = processingPlayerStatsService;
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void PutOnAnItem(ItemData itemData)
        {
            if (itemData == null)
                return;
            
            UpdateStaminaCost(itemData, _processingPlayerStatsService.UpStatEquipment);
            ApplyStats(itemData.MainStats);
            ApplyStats(itemData.AdditionalStats);
        }

        public void RemoveAnItem(ItemData itemData)
        {
            if (itemData == null)
                return;

            UpdateStaminaCost(itemData, _processingPlayerStatsService.DownStatEquipment);
            CancelStats(itemData.MainStats);
            CancelStats(itemData.AdditionalStats);
        }

        private void ApplyStats(List<StatItemData> stats)
        {
            if (stats == null)
                return;
            
            foreach (StatItemData statData in stats)
            {
                switch (statData.Type)
                {
                    case StatType.Health:
                        _processingPlayerStatsService.UpHealthEquipment(statData.Value);
                        break;

                    default:
                        _processingPlayerStatsService.UpStatEquipment(statData.Type, statData.Value);
                        break;
                }
            }
        }

        private void CancelStats(List<StatItemData> stats)
        {
            if (stats == null)
                return;
            
            foreach (StatItemData statData in stats)
            {
                switch (statData.Type)
                {
                    case StatType.Health:
                        _processingPlayerStatsService.DownHealthEquipment(statData.Value);
                        break;

                    default:
                        _processingPlayerStatsService.DownStatEquipment(statData.Type, statData.Value);
                        break;
                }
            }
        }

        private void UpdateStaminaCost(ItemData itemData, Action<StatType, float> onUpdate)
        {
            float cost = itemData.Type switch
            {
                ItemType.Sword or ItemType.Mace => 1f,
                ItemType.Dagger => 0.35f,
                ItemType.TwoHandedWeapon => 1.5f,
                _ => 0f
            };

            onUpdate(StatType.StaminaCost, cost);
        }
    }
}