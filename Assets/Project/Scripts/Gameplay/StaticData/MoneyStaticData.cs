using UnityEngine;

using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "Money_Data", menuName = "Static data/Money")]
    public class MoneyStaticData : ItemStaticData
    {
        public MoneyType moneyType;
    }
}