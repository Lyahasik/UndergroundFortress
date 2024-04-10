using UnityEngine;

namespace UndergroundFortress.UI.Inventory
{
    [CreateAssetMenu(fileName = "Purchase_Data", menuName = "Static data/Purchases")]
    public class PurchaseStaticData : ScriptableObject
    {
        public int id;
        public Sprite icon;
        public MoneyType moneyType;
        public float price;

        public RewardData rewardData;
    }
}