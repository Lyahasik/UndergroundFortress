using System;
using UnityEngine;

using UndergroundFortress.Gameplay;
using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.UI.Inventory
{
    [Serializable]
    public class ItemNumberData
    {
        public ItemStaticData itemData;
        public int number;
        public int level;

        [Space]
        public QualityType qualityType;
    }
}