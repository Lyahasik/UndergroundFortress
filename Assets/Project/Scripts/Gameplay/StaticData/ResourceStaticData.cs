﻿using UnityEngine;

using UndergroundFortress.Constants;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class ResourceStaticData : ItemStaticData
    {
        public string name;
        
        [Space]
        public string description;
        public QualityType quality;

        [Space]
        public int maxNumberForCell = ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
    }
}