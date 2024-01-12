using UnityEngine;

using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items
{
    public class StatItemData
    {
        private readonly StatType _type;
        private readonly Sprite _icon;
        private readonly float _value;
        
        public StatType Type => _type;
        public Sprite Icon => _icon;
        public float Value => _value;

        public StatItemData(StatType type, Sprite icon, float value)
        {
            _type = type;
            _icon = icon;
            _value = value;
        }
    }
}