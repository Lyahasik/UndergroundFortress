using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface IInventoryService : IService
    {
        public Dictionary<int, CellData[]> Bags { get; }

        public void Initialize();
        public void AddItem(ItemData itemData);
    }
}