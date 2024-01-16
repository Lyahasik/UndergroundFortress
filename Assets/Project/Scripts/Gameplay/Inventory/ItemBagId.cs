namespace UndergroundFortress.Gameplay.Inventory
{
    public struct ItemBagId
    {
        public int BagId;
        public int CellId;

        public ItemBagId(in int bagId, in int cellId)
        {
            BagId = bagId;
            CellId = cellId;
        }
    }
}