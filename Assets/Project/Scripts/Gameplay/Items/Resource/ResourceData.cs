namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ResourceData : ItemData
    {
        private string _description;
        public string Description => _description;

        public ResourceData(int id,
            string name,
            ItemType type,
            QualityType qualityType,
            int maxNumberForCell,
            string description)
        {
            Id = id;
            Name = name;
            Type = type;
            QualityType = qualityType;
            MaxNumberForCell = maxNumberForCell;
            _description = description;
        }
    }
}