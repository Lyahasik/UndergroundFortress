using UnityEngine;

namespace UndergroundFortress.Gameplay.Items
{
    public abstract class ItemData
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract ItemType Type { get; }
        public abstract Sprite Icon { get; }
        public abstract QualityType QualityType { get; }
        public abstract int MaxNumberForCell { get; }
    }
}