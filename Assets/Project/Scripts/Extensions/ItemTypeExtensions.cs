using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Extensions
{
    public static class ItemTypeExtensions
    {
        public static bool IsEquipment(this ItemType type) => 
            type is >= ItemType.Sword and < ItemType.Resource;
        public static bool IsResource(this ItemType type) => 
            type >= ItemType.Resource;
    }
}