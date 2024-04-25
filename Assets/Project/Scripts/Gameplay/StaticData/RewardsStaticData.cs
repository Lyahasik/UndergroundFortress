using System.Collections.Generic;
using UnityEngine;

using UndergroundFortress.UI.Inventory;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "RewardsData", menuName = "Static data/Rewards")]
    public class RewardsStaticData : ScriptableObject
    {
        [Header("Accumulation reward")]
        public int numberCoinsByLevel;
        public int numberToDisplayByLevel;
        public float delayAccrualSeconds;
        public float maxAccumulationSeconds;

        [Header("Daily reward")]
        public List<RewardData> dailyRewards;
    }
}