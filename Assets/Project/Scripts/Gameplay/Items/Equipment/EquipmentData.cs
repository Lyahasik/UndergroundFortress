using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items.Equipment
{
    public class EquipmentData
    {
        private int _level;
        private bool _isSet;
        private QualityType _quality;
        private string _name;
        private Sprite _icon;

        [Space]
        private List<StatItemData> _mainStats;
        private List<StatItemData> _additionalStats;

        private List<StoneItemData> _stones;

        public int Level => _level;
        public bool IsSet => _isSet;
        public QualityType Quality => _quality;
        public string Name => _name;
        public Sprite Icon => _icon;

        public List<StatItemData> MainStats => _mainStats;
        public List<StatItemData> AdditionalStats => _additionalStats;
        
        public List<StoneItemData> Stones => _stones;

        public EquipmentData(int level,
            in QualityType quality,
            string name,
            Sprite icon,
            List<StatItemData> mainStats,
            List<StatItemData> additionalStats,
            List<StoneItemData> stones,
            in bool isSet)
        {
            _level = level;
            _isSet = isSet;
            _quality = quality;
            _name = name;
            _icon = icon;
            _mainStats = mainStats;
            _additionalStats = additionalStats;
            _stones = stones;
        }
    }
}