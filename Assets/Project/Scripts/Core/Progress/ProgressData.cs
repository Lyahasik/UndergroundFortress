using System.Collections.Generic;

using UndergroundFortress.Gameplay.Inventory;
using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Core.Progress
{
    public class ProgressData
    {
        public int Level;
        public Dictionary<StatType, float> MainStats;

        public WalletData Wallet;
        
        public Dictionary<ItemType, List<int>> ActiveRecipes;
        public List<CellData> Equipment;
        public List<CellData> Bag;
        public int FilledNumberBag;
    }
}