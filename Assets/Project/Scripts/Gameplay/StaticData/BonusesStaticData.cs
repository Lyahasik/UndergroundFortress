using System.Collections.Generic;
using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "BonusesData", menuName = "Static data/Bonuses")]
    public class BonusesStaticData : ScriptableObject
    {
        public int unlockLevel;
        public float sentenceOffers;
        public List<BonusData> bonusesData;
    }
}