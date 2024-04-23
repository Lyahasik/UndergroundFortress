using UnityEngine;

using UndergroundFortress.Gameplay;

namespace UndergroundFortress.UI.Inventory
{
    [CreateAssetMenu(fileName = "Purchase_Data", menuName = "Static data/Purchases")]
    public class PurchaseStaticData : ScriptableObject
    {
        public int id;
        public Sprite icon;
        public QualityType qualityType;
        public MoneyType moneyType;
        public int price;
        public int rewardIdAds;

        public int dungeonIdUnlock;
        public int dungeonIdLock;

        public RewardData rewardData;
    }
}