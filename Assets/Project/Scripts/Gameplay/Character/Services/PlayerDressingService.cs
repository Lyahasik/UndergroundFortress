using System.Collections.Generic;

using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Character.Services;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
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
        
        public void DressThePlayer(CharacterData characterData, List<EquipmentData> items)
        {
            foreach (EquipmentData item in items)
            {
                PutOnAnItem(characterData, item);
            }
        }

        public void PutOnAnItem(CharacterData characterData, EquipmentData itemData)
        {
            foreach (StatItemData statData in itemData.AdditionalStats)
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

        public void RemoveAnItem(CharacterData characterData, EquipmentData itemData)
        {
            foreach (StatItemData statData in itemData.AdditionalStats)
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