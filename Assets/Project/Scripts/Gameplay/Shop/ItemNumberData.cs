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

        [Space]
        public QualityType qualityType;
    }
}