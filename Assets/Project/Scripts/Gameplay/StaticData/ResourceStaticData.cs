using UnityEngine;

using UndergroundFortress.Constants;
using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Static data/Resource")]
    public class ResourceStaticData : ScriptableObject
    {
        public int id;
        public ItemType type;
        
        [Space]
        public string name;
        public string description;
        public QualityType quality;
        public Sprite icon;
        public int maxNumberForCell = ConstantValues.MIN_NUMBER_ITEM_FOR_CELL;
    }
}