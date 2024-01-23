using System.Collections.Generic;

using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Core.Progress
{
    public class ProgressData
    {
        public int Level;
        
        public Dictionary<ItemType, List<int>> ActiveRecipes;
        public List<CellData> Equipment;
        public List<CellData> Bag;
    }
}