using System;

namespace UndergroundFortress.Gameplay.Stats
{
    [Serializable]
    public struct QualityValue
    {
        public QualityType qualityType;
        public float minValue;
        public float maxValue;

        public QualityValue(QualityType qualityType, float minValue, float maxValue)
        {
            this.qualityType = qualityType;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}