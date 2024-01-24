using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Extensions
{
    public static class ItemTypeExtensions
    {
        public static bool IsEquipment(this ItemType type) => 
            type is >= ItemType.Weapon and < ItemType.Resource;
    }
}