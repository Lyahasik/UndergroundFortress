using UndergroundFortress.Gameplay.StaticData;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ConsumableResourceData : ResourceData
    {
        public ConsumableResourceData(int id,
            string name,
            ItemType type,
            QualityType qualityType,
            ConsumableType consumableType) : base(id, name, type, qualityType)
        {
            Id = id;
            Name = name;
            Type = type;
            QualityType = qualityType;
            ConsumableType = consumableType;
        }
    }
}