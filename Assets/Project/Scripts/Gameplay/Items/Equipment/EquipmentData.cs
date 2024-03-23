using System.Collections.Generic;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentData : ItemData
    {
        private int _level;
        private bool _isSet;

        private List<StatItemData> _mainStats;
        private List<StatItemData> _additionalStats;

        private List<StoneItemData> _stones;
        public int Level => _level;
        public bool IsSet => _isSet;

        public List<StatItemData> MainStats => _mainStats;
        public List<StatItemData> AdditionalStats => _additionalStats;

        public List<StoneItemData> Stones => _stones;

        public EquipmentData(int id,
            string name,
            ItemType type,
            in QualityType quality,
            int maxNumberForCell,
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
            MaxNumberForCell = maxNumberForCell;
            
            _level = level;
            _isSet = isSet;
            
            _mainStats = mainStats;
            _additionalStats = additionalStats;
            _stones = stones;
        }
    }
}