using System;

namespace UndergroundFortress.Gameplay.Stats
{
    [Serializable]
    public struct QualityValue
    {
        public QualityType qualityType;
        public float minValue;
        public float maxValue;
    }
}