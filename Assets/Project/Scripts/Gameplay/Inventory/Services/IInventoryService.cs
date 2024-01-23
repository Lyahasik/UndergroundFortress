using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface IInventoryService : IService
    {
        public List<CellData> Bag { get; }
        public List<CellData> Equipment { get; }

        public void AddItem(ItemData itemData);
    }
}