using UnityEngine;

namespace UndergroundFortress.Core.Progress
{
    public class RewardsData
    {
        public long LastCalculateTime;
        public int NumberCoins;

        [Space]
        public int LastAwardId;
        public RewardDate LastDateAward;
    }
}