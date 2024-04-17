using UnityEngine;

using UndergroundFortress.Gameplay.Items;

namespace UndergroundFortress.Gameplay.StaticData
{
    public class ItemStaticData : ScriptableObject
    {
        public int id;
        public Sprite icon;
        public ItemType type;
        
        [Space]
        public int priceTime;
        public int probabilityWeight;
    }
}