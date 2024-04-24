using System;
using System.Collections.Generic;

namespace UndergroundFortress.Gameplay.StaticData
{
    [Serializable]
    public class RecipeLevelPrice
    {
        public int level;
        public int money1;
        public List<PriceResourceData> resourcesPrice;
    }
}