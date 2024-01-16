using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items
{
    public abstract class ItemData
    {
        public abstract int Id { get; }
        public abstract ItemType Type { get; }
        public abstract QualityType Quality { get; }
        public abstract int MaxNumberForCell { get; }
    }
}