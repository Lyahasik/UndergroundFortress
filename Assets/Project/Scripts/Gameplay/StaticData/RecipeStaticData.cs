using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class RecipeStaticData : ScriptableObject
    {
        public int idItem;

        [Space] [Header("Price")]
        public int money1;
        public List<PriceResourceData> resourcesPrice;
    }
}