using System.Collections.Generic;

using UndergroundFortress.Gameplay.Items;
using UndergroundFortress.Gameplay.Items.Equipment;

namespace UndergroundFortress.Core.Progress
{
    public class ProgressData
    {
        public int Level;
        
        public List<EquipmentData> PlayerEquipments;
        public Dictionary<ItemType, List<int>> ActiveRecipes;
    }
}