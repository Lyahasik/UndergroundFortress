namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ResourceData : ItemData
    {
        public ResourceData(int id,
            string name,
            ItemType type,
            QualityType qualityType)
        {
            Id = id;
            Name = name;
            Type = type;
            QualityType = qualityType;
        }
    }
}