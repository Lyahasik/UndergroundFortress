using System;
using UnityEngine;

using UndergroundFortress.Gameplay;

namespace UndergroundFortress.UI.Inventory
{
    [Serializable]
    public class ItemNumberData
    {
        public int itemId;
        public int number;
        public int level;

        [Space]
        public QualityType qualityType;
    }
}