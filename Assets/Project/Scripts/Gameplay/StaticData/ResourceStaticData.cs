using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class ResourceStaticData : ItemStaticData
    {
        public ItemType type;
        public string name;
        
        [Space]
        public string description;
        public QualityType quality;
        public Sprite icon;
        public int maxNumberForCell = ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
    }
}