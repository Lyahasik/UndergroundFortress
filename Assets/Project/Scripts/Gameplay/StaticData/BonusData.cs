using System;
using UnityEngine;

using UndergroundFortress.Core.Services.Bonuses;

namespace UndergroundFortress.Gameplay.StaticData
{
    [Serializable]
    public class BonusData
    {
        public string name;
        public string description;
        public Sprite iconOffer;
        public float lifetimeOffer;
        public int rewardIdAds;
        
        public BonusType bonusType;
        public int probabilityWeight;
        public float value;
        public float lifetimeBonus;
    }
}