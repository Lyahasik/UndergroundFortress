using System.Collections.Generic;

using UndergroundFortress.Core.Services;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.Inventory.Services
{
    public interface IInventoryService : IService
    {
        public List<CellData> Bag { get; }

        public void Initialize();
        public void AddItem(ItemData itemData);
        public void SwapItems(in int id1, in int id2);
    }
}