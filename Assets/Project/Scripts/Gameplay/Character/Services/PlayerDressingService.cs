using System.Collections.Generic;

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
        }

        public void PutOnAnItem(ItemData itemData)
        {
            if (itemData == null)
                return;
            
            ApplyStats(itemData.MainStats);
            ApplyStats(itemData.AdditionalStats);
        }

        public void RemoveAnItem(ItemData itemData)
        {
            if (itemData == null)
                return;
            
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
                        _processingPlayerStatsService.UpHealth(statData.Value);
                        break;

                    default:
                        _processingPlayerStatsService.UpStat(statData.Type, statData.Value);
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
                        _processingPlayerStatsService.DownHealth(statData.Value);
                        break;

                    default:
                        _processingPlayerStatsService.DownStat(statData.Type, statData.Value);
                        break;
                }
            }
        }
    }
}