using System.Collections.Generic;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentData : ItemData
    {
        private List<StoneItemData> _stones;

        public List<StoneItemData> Stones => _stones;

        public EquipmentData(int id,
            string name,
            ItemType type,
            in QualityType quality,
            int level,
            in bool isSet,
            List<StatItemData> mainStats,
            List<StatItemData> additionalStats,
            List<StoneItemData> stones)
        {
            Id = id;
            Name = name;
            Type = type;
            QualityType = quality;
            
            Level = level;
            IsSet = isSet;
            
            MainStats = mainStats;
            AdditionalStats = additionalStats;
            _stones = stones;
        }
    }
}