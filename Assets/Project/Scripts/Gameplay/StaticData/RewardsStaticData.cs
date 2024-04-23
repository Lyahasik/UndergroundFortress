using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "RewardsData", menuName = "Static data/Rewards")]
    public class RewardsStaticData : ScriptableObject
    {
        public int numberCoinsByLevel;
        public int numberToDisplayByLevel;
        public float delayAccrualSeconds;
        public float maxAccumulationSeconds;
    }
}