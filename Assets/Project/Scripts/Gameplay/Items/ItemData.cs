using System.Collections.Generic;

namespace UndergroundFortress.Gameplay.Items
{
    public class ItemData
    {
        public int Id;
        public string Name;
        public ItemType Type;
        public QualityType QualityType;
        
        public int Level;
        public bool IsSet;
        
        public List<StatItemData> MainStats;
        public List<StatItemData> AdditionalStats;
    }
}