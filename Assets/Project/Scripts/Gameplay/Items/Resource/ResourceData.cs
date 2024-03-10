using UnityEngine;

namespace UndergroundFortress.Gameplay.Items.Resource
{
    public class ResourceData : ItemData
    {
        private int _id;
        private ItemType _type;
        
        private string _name;
        private string _description;
        private QualityType _qualityType;
        private Sprite _icon;
        private int _maxNumberForCell;

        public override int Id => _id;
        public override ItemType Type => _type;

        public override string Name => _name;
        public string Description => _description;
        public override QualityType QualityType => _qualityType;
        public override Sprite Icon => _icon;
        public override int MaxNumberForCell => _maxNumberForCell;

        public ResourceData(int id,
            ItemType type,
            string name,
            string description,
            QualityType qualityType,
            Sprite icon,
            int maxNumberForCell)
        {
            _id = id;
            _type = type;
            _name = name;
            _description = description;
            _qualityType = qualityType;
            _icon = icon;
            _maxNumberForCell = maxNumberForCell;
        }
    }
}