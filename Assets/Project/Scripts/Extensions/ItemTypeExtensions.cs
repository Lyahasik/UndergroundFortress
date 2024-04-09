using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Extensions
{
    public static class ItemTypeExtensions
    {
        public static bool IsEquipment(this ItemType type) => 
            type is >= ItemType.Sword and < ItemType.Resource;
        public static bool IsBaseEquipment(this ItemType type) => 
            type is ItemType.Sword
                or ItemType.Chest
                or ItemType.Pants
                or ItemType.Boots
                or ItemType.Gloves;
        public static bool IsResource(this ItemType type) => 
            type >= ItemType.Resource;
    }
}