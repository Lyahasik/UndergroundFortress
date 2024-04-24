using System;
using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class RecipeStaticData : ScriptableObject
    {
        public ItemStaticData itemData;

        [Space] [Header("Levels price")]
        public List<RecipeLevelPrice> levelsPrice;

        public RecipeLevelPrice GetLevelPrice(int level)
        {
            int clampLevel = Math.Clamp(level, levelsPrice[0].level, levelsPrice[4].level);
            return levelsPrice.Find(data => data.level == clampLevel);
        }
    }
}