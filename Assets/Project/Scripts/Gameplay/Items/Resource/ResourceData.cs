using UndergroundFortress.Gameplay.Stats;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ResourceData : ItemData
    {
        private int _id;
        private ItemType _type;
        
        private string _name;
        private QualityType _quality;
        private int _maxNumberForCell;

        public override int Id => _id;
        public override ItemType Type => _type;

        public string Name => _name;
        public override QualityType Quality => _quality;
        public override int MaxNumberForCell => _maxNumberForCell;

        public ResourceData(int id, ItemType type, string name, QualityType quality, int maxNumberForCell)
        {
            _id = id;
            _type = type;
            _name = name;
            _quality = quality;
            _maxNumberForCell = maxNumberForCell;
        }
    }
}