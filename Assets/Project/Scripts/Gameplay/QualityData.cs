using System;
using UnityEngine;

namespace UndergroundFortress.Gameplay
{
    [Serializable]
    public class QualityData
    {
        public QualityType type;
        public float pricePercentage;
        public int weight;
        public Sprite background;
    }
}