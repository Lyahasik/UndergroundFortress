using System.Collections.Generic;
using UndergroundFortress.Gameplay.Items;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class RecipeStaticData : ScriptableObject
    {
        public int idItem;

        public List<PriceResourceData> resourcesPrice;
    }
}