using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentData : ItemData
    {
        private int _id;
        private ItemType _type;
        
        private int _level;
        private bool _isSet;
        private QualityType _quality;
        private string _name;
        private Sprite _icon;
        private int _maxNumberForCell;

        private List<StatItemData> _mainStats;
        private List<StatItemData> _additionalStats;

        private List<StoneItemData> _stones;

        public override int Id => _id;
        public override ItemType Type => _type;
        public int Level => _level;
        public bool IsSet => _isSet;
        public override QualityType Quality => _quality;
        public string Name => _name;
        public Sprite Icon => _icon;
        public override int MaxNumberForCell => _maxNumberForCell;

        public List<StatItemData> MainStats => _mainStats;
        public List<StatItemData> AdditionalStats => _additionalStats;

        public List<StoneItemData> Stones => _stones;


        public EquipmentData(int id,
            ItemType type,
            int level,
            in bool isSet,
            in QualityType quality,
            string name,
            Sprite icon,
            int maxNumberForCell,
            List<StatItemData> mainStats,
            List<StatItemData> additionalStats,
            List<StoneItemData> stones)
        {
            _id = id;
            _type = type;
            
            _level = level;
            _isSet = isSet;
            _quality = quality;
            _name = name;
            _icon = icon;
            _maxNumberForCell = maxNumberForCell;
            
            _mainStats = mainStats;
            _additionalStats = additionalStats;
            _stones = stones;
        }
    }
}