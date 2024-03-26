using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items
{
    public class StatItemData
    {
        private readonly StatType _type;
        private readonly float _value;
        private readonly QualityType _qualityType;

        public StatType Type => _type;
        public QualityType QualityType => _qualityType;
        public float Value => _value;

        public StatItemData(StatType type, QualityType qualityType, float value)
        {
            _type = type;
            _qualityType = qualityType;
            _value = value;
        }
    }
}