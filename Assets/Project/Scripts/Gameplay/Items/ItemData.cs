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

        public static bool operator ==(ItemData item1, ItemData item2)
        {
            if (item1 is null)
                return item2 is null;
            
            return item1.Equals(item2);
        }

        public static bool operator !=(ItemData item1, ItemData item2) => 
            !(item1 == item2);

        public override bool Equals(object obj)
        {
            var item = obj as ItemData;

            if (item == null)
                return false;

            return Id == item.Id && Name == item.Name;
        }

        public override int GetHashCode() => 
            Id.GetHashCode() + Name.GetHashCode();
    }
}