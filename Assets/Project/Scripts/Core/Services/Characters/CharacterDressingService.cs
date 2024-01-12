using System.Collections.Generic;

using UndergroundFortress.Gameplay.Character;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Core.Services.Characters
{
    public class CharacterDressingService : ICharacterDressingService
    {
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
                        characterData.Stats.UpHealth(statData.Value);
                        break;
                
                    default:
                        characterData.Stats.UpStat(statData.Type, statData.Value);
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
                        characterData.Stats.DownHealth(statData.Value);
                        break;
                
                    default:
                        characterData.Stats.DownStat(statData.Type, statData.Value);
                        break;
                }
            }
        }
    }
}