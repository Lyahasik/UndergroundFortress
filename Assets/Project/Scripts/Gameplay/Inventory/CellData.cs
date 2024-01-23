using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.Inventory
{
    public class CellData
    {
        public ItemData ItemData;
        public int Number;

        public CellData() {}

        public CellData(ItemData itemData, int number)
        {
            ItemData = itemData;
            Number = number;
        }
    }
}